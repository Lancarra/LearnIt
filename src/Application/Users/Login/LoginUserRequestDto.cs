using MediatR;

namespace Application.Users.Login
{
    public class LoginUserRequestDto : IRequest<LoginUserResponseDto>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string GoogleAuthCode { get; set; }

        public bool FromOtherService { get; set; }

        public string GoogleAuthKey { get; set; }
        public string VerificationString { get; set; }
    }
}
