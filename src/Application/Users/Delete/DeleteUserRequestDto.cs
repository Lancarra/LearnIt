using MediatR;

namespace Application.Users.Delete;

public class DeleteUserRequestDto : IRequest<DeleteUserResponseDto>
{
    public int UserId { get; set; }
}