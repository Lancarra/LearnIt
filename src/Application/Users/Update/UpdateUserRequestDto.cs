using MediatR;

namespace Application.Users.Update;

public class UpdateUserRequestDto : IRequest<UpdateUserResponseDto>
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
}