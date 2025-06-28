using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetRole;

public class GetRoleHandler : IRequestHandler<GetRoleRequestDto, GetRoleResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetRoleHandler(LearnContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetRoleResponseDto> Handle(GetRoleRequestDto request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles.Select(r => new GetRoleViewModel
            { RoleId = r.RoleId,
            RoleName = r.RoleName,
            RoleDescription = r.RoleDescription}).ToListAsync(cancellationToken);
        
        return new GetRoleResponseDto{Roles = roles};
    }
}