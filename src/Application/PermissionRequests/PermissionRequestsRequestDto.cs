using MediatR;

namespace Application.PermissionRequests;

public class PermissionRequestsRequestDto : IRequest<PermissionRequestsResponseDto>
{
    public int RequestUserId { get; set; }
    public string RoleName { get; set; }

}