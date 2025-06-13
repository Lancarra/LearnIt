using MediatR;

namespace Application.Users.Create
{
    public class CreateUserRequestDto : IRequest<CreateUserResponseDto>
    {
        public string Email { get; set; }

        public string Password { get; set; }
        public string Username { get; set; } 
    }
}
