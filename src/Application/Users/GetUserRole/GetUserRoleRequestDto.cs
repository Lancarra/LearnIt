using MediatR;

namespace Application.Users.GetUserRole;

public class GetUserRoleRequestDto : IRequest<GetUserRoleResponseDto>
{
    public int UserId { get; set; }
}