using API.Controllers.Users.Constants;
using Application.Definition.Create;
using Application.Definition.Delete;
using Application.Definition.Get;
using Application.Definition.GetAll;
using Application.Definition.GetById;
using Application.Definition.Update;
using Application.Users.GetById;
using Application.Users.Update;
using HtmlAgilityPack;
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

    [HttpPut("update-definition-image")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<string> GetImageUrlFromGoogle(string searchTerm, int imageIndex = 3)
    {
        string url = $"https://www.google.com/search?tbm=isch&q={Uri.EscapeDataString(searchTerm)}";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string html = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var imgNodes = doc.DocumentNode.SelectNodes("//img[@src and not(starts-with(@src, 'data:'))]");
            if (imgNodes != null && imgNodes.Count > imageIndex)
            {
                string src = imgNodes[imageIndex].GetAttributeValue("src", null);
                if (!string.IsNullOrEmpty(src))
                {
                    if (src.StartsWith("http"))
                    {
                        /*
                        src = "https://www.google.com" + src;
                        */
                        return src;
                    }
                }
            }

            return null;
        }
    }
    
    #region Blob
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
    
    
    [HttpPost("blob/update-url/{defintionId:guid}")]
    public async Task<IActionResult> UpdateDocumentFileUrl([FromRoute] Guid defintionId, string imageUrl)
    {
        Guid blobId = Guid.Empty;

        try
        {
            var response = await _mediator.Send(new GetDefinitionByIdRequestDto { DefinitionId = defintionId });

            if (response.BlobId != null)
            {
                await _blobService.DeleteAsync((Guid)response.BlobId, Constants.DEFINITION_CONTAINER);
            }

            using (HttpClient client = new HttpClient())
            {
                var image = await client.GetStreamAsync(imageUrl);
                await using var stream = image; 
                blobId = await _blobService.UploadAsync(stream, "image/jpeg", Constants.DEFINITION_CONTAINER);
            }
            
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
    #endregion  
}