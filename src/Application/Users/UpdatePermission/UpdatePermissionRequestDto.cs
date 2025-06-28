using MediatR;

namespace Application.Users.UpdatePermission;

public class UpdatePermissionRequestDto : IRequest<UpdatePermissionResponseDto>
{
    public int RoleId { get; set; }
    public int UserId { get; set; }
}