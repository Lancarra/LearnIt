using Application.Definition.Create;
using Application.Definition.Delete;
using Application.Definition.Get;
using Application.Definition.GetAll;
using Application.Definition.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Definition;

[ApiController]
[Route("definition")]

public class DefinitionController:ControllerBase
{
    private readonly IMediator _mediator;
    public DefinitionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-all-definitions")]
    public async Task<GetDefinitionResponseDto> GetDefinition([FromBody] GetDefinitionRequestDto request)
    {
        var definitions = await _mediator.Send(request);
        return definitions;
    }
    
    [HttpPost("create-definition")]
    public async Task<CreateDefinitionResponseDto> CreateDefinition([FromBody] CreateDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        return definition;
    }
    
    [HttpPut("update-definition")]
    public async Task<UpdateDefinitionResponseDto> UpdateDefinition([FromBody] UpdateDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        return definition;
    }
    
    [HttpDelete("delete-definition")]
    public async Task<DeleteDefinitionResponseDto> DeleteDefinition([FromBody] DeleteDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        return definition;
    }
        
}