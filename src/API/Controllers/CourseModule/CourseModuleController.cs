using Application.CourseModule.Create;
using Application.CourseModule.Delete;
using Application.CourseModule.GetAll;
using Application.CourseModule.GetByTeacherId;
using Application.CourseModule.Update;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CourseModule;

[Authorize]
[ApiController]
[Route("course-module")]
public class CourseModuleController: ControllerBase
{
    private readonly IMediator _mediator;
    public CourseModuleController(IMediator mediator)
    {
        _mediator = mediator;
    }
    


    [HttpGet("get-all-modules")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<GetModuleResponseDto> GetModule()
    {
        var modules = await _mediator.Send(new GetModuleRequestDto());;
        return modules;
    }
    
    [HttpGet("get-by-teacher-id-modules/{teacherId:int}")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<GetByTeacherIdResponseDto> GetByTeacherIdModule([FromRoute] int teacherId)
    {
        var modules = await _mediator.Send(new GetByTeacherIdRequestDto(){TeacherId = teacherId});
        return modules;
    }
    
    [HttpPost("create-module")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CreateModuleResponseDto> CreateModule([FromBody] CreateModuleRequestDto request)
    {
        var module = await _mediator.Send(request);
        return module;
    }
    
    [HttpPut ("update-module")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<UpdateModuleResponseDto> UpdateModule([FromBody] UpdateModuleRequestDto request)
    {
        var module = await _mediator.Send(request);
        return module;
    }
    
    [HttpDelete ("delete-module")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<DeleteModuleResponseDto> DeleteModule([FromBody] DeleteModuleRequestDto request)
    {
        var module = await _mediator.Send(request);
        return module;
    }
    
    
}