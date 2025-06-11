using Application.Definition.Create;
using Application.Definition.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Definition;

[ApiController]
[Route("Definition")]

public class DefinitionController:ControllerBase
{
    private readonly IMediator _mediator;
    public DefinitionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateDefinition")]
    public async Task<CreateDefinitionResponseDto> CreateDefinition([FromBody] CreateDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        return definition;
    }
    
    [HttpPut("UpdateDefinition")]
    public async Task<UpdateDefinitionResponseDto> UpdateDefinition([FromBody] UpdateDefinitionRequestDto request)
    {
        var definition = await _mediator.Send(request);
        return definition;
    }
}