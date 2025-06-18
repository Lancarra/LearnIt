using API.Controllers.Users.Verify;
using Application.Users._2FaUth.Disable;
using Application.Users._2FaUth.Generate;
using Application.Users._2FaUth.Verify;
using Application.Users.Delete;
using Application.Users.Read;
using Application.Users.Update;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    [ApiController]
    [Route("user")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public UserController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        public async Task<ReadUserResponseDTO> GetCurrent()
        {
            var result = await _mediator.Send(new ReadUserRequestDTO(_currentUserAccessor.GetCurrentEmail() ?? string.Empty));

            return result;
        }

        [HttpPut]
        public async Task<UpdateUserResponseDto> UpdateUser([FromBody] UpdateUserRequestDto request)
        {
            var result = await _mediator.Send(new UpdateUserRequestDto()
            {
                Email = request.Email,
                Password = request.Password
            });

            return result;
        }

        [HttpPut("Generate2FactorAuth")]
        public async Task<Generate2FAuthResponseDTO> Generate2FactorAuth([FromQuery] bool retry, [FromBody] Generate2FAuthRequestDTO request)
        {
            var result = await _mediator.Send(new Generate2FAuthRequestDTO() { Retry = retry, Password = request.Password });
            return result;
        }

        [HttpPut("Disable2FactorAuth")]
        public async Task<Unit> Disable2FactorAuth([FromBody] Disable2FAuthRequestDTO request)
        {
            return await _mediator.Send(new Disable2FAuthRequestDTO() { Password = request.Password, GoogleCode = request.GoogleCode });
        }

        [HttpPut("Verify2FactorAuth")]
        public async Task<Unit> VerifyFactorAuth([FromBody] Verify2FAuthRequest request)
        {
            return await _mediator.Send(new Verify2FAuthRequestDTO() { GoogleAuthCode = request.GoogleAuthCode });
        }

        [HttpDelete("{email}")]
        public async Task<bool> DeleteUser([FromRoute] string email)
        {
            return await _mediator.Send(new DeleteUserRequestDto(){Email = email});
        }
    }
}