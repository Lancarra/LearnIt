using Application.Definition.Create;
using Application.Definition.Delete;
using Application.Definition.Get;
using Application.Definition.GetAll;
using Application.Definition.Update;
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
    public DefinitionController(IMediator mediator)
    {
        _mediator = mediator;
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
        return definition;
    }
        
}