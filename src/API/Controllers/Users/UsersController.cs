using Application.Users._2FaUth.CheckIfEnabled;
using Application.Users.Create;
using Application.Users.Login;
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
    
    [HttpPost("create-user")]
    public async Task<CreateUserResponseDto> Create([FromBody] CreateUserRequestDto request)
    {
        return await _mediator.Send(request);
    } 
    
    [HttpGet("Check2Factor/{email}")]
    public async Task<bool> Check2Factor([FromRoute] string email)
    {
        return await _mediator.Send(new Check2FactorRequestDto() { Email = email });
    }
    
    [HttpPost("login")]
    public async Task<LoginUserResponseDto> Login([FromBody] LoginUserRequestDto request)
    {
        return await _mediator.Send(request);
    }
}