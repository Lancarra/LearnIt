using Application.Users._2FaUth.CheckIfEnabled;
using Application.Users.Create;
using Application.Users.Delete;
using Application.Users.GetById;
using Application.Users.Login;
using Application.Users.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-user-by-id/{userId:int}")]
    public async Task<GetUserResponseDto> Get([FromRoute] int userId)
    {
        var user = await _mediator.Send(new GetUserRequestDto { UserId = userId });
        return user;
    }
    
    [HttpPost("create-user")]
    public async Task<CreateUserResponseDto> Create([FromBody] CreateUserRequestDto request)
    {
        return await _mediator.Send(request);
    } 
    
    [HttpGet("check2factor/{email}")]
    public async Task<bool> Check2Factor([FromRoute] string email)
    {
        return await _mediator.Send(new Check2FactorRequestDto() { Email = email });
    }
    
    [HttpPost("login")]
    public async Task<LoginUserResponseDto> Login([FromBody] LoginUserRequestDto request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpPut("update-user")]
    public async Task<UpdateUserResponseDto> Update([FromBody] UpdateUserRequestDto request)
    {
        return await _mediator.Send(request);
    }

    [HttpDelete("delete-user")]
    public async Task<DeleteUserResponseDto> Delete([FromBody] DeleteUserRequestDto request)
    {
        return await _mediator.Send(request);
    }
}