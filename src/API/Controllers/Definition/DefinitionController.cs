using API.Controllers.Users.Constants;
using Application.Definition.Create;
using Application.Definition.Delete;
using Application.Definition.Get;
using Application.Definition.GetAll;
using Application.Definition.GetById;
using Application.Definition.Update;
using Application.Users.GetById;
using Application.Users.Update;
using Infrastructure.BlobStorage.Service;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Definition;

[Authorize]
[ApiController]
[Route("definition")]

public class DefinitionController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBlobService _blobService;
    public DefinitionController(IMediator mediator, IBlobService blobService)
    {
        _mediator = mediator;
        _blobService = blobService;
    }

    [HttpGet("get-all-definitions/{dictionaryId}")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<GetDefinitionResponseDto> GetDefinition([FromRoute] Guid dictionaryId)
    {
        var response = await _mediator.Send(new GetDefinitionRequestDto
        {
            DictionaryId = dictionaryId
        });
        return response;
    }

    
    [HttpPost("create-definition")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CreateDefinitionResponseDto> CreateDefinition([FromBody] CreateDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        return definition;
    }
    
    [HttpPut("update-definition")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<UpdateDefinitionResponseDto> UpdateDefinition([FromBody] UpdateDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        return definition;
    }
    
    [HttpDelete("delete-definition")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<DeleteDefinitionResponseDto> DeleteDefinition([FromBody] DeleteDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        await _blobService.DeleteAsync((Guid)definition.BlobId, Constants.DEFINITION_CONTAINER);
        return definition;
    }
    
    [HttpGet("blob/get/{fileBlobId:guid}")]
    public async Task<IActionResult> DownloadFileBlob(Guid fileBlobId)
    {
        try
        {
            var result = await _blobService.DownloadAsync(fileBlobId, Constants.DEFINITION_CONTAINER);
            if (result == null || result.stream == null)
            {
                return NotFound("Image not found");
            }
            return File(result.stream, result.contentType);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
    
    [HttpPost("blob/update/{defintionId:guid}")]
    public async Task<IActionResult> UpdateDocumentFile([FromRoute] Guid defintionId, IFormFile file)
    {
        Guid blobId = Guid.Empty;

        try
        {
            var response = await _mediator.Send(new GetDefinitionByIdRequestDto { DefinitionId = defintionId });

            if (response.BlobId != null)
            {
                await _blobService.DeleteAsync((Guid)response.BlobId, Constants.DEFINITION_CONTAINER);
            }
            await using var stream = file.OpenReadStream(); 
            blobId = await _blobService.UploadAsync(stream, file.ContentType, Constants.DEFINITION_CONTAINER);

            var updateResponse = await _mediator.Send(new UpdateDefinitionRequestDto
            {
                Id = response.Id,
                Word = response.Word,
                Meaning = response.Meaning,
                DictionaryId = response.DictionaryId,
                BlobId = blobId
            });
            return Ok(updateResponse);
        }
        catch (Exception exception)
        {
            await _blobService.DeleteAsync(blobId, Constants.DEFINITION_CONTAINER);
            return BadRequest(exception.Message);
        }
    }
        
}