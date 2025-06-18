using MediatR;

namespace Application.Users.Read
{
    public class ReadUserRequestDTO : IRequest<ReadUserResponseDTO>
    {
        public ReadUserRequestDTO(string email)
        {
            Email = email;
        }
        public string Email { get; set; }
    }
}
