using Application.CourseModule.Create;
using Application.CourseModule.Update;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CourseModule;

[Authorize]
[ApiController]
[Route("CourseModule")]
public class CourseModuleController: ControllerBase
{
    private readonly IMediator _mediator;
    public CourseModuleController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateModule")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CreateModuleResponseDto> CreateModule([FromBody] CreateModuleRequestDto request)
    {
        var module = await _mediator.Send(request);
        return module;
    }
    
    [HttpPut ("UpdateModule")]
    public async Task<UpdateModuleResponseDto> UpdateModule([FromBody] UpdateModuleRequestDto request)
    {
        var module = await _mediator.Send(request);
        return module;
    }
    
    
}