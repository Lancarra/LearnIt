using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetUserRole;

public class GetUserRoleHandler : IRequestHandler<GetUserRoleRequestDto, GetUserRoleResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetUserRoleHandler(LearnContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }


    public async Task<GetUserRoleResponseDto> Handle(GetUserRoleRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Where(x => x.Email == _currentUserAccessor.GetCurrentEmail() && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        PropertyChecker.CheckNullAndThrow404(user);
        
        var roles = await _context.UserRole.Include(ur => ur.Role)
                                            .Where(ur => ur.UserId == request.UserId)
                                            .Select(ur => ur.Role.RoleName)
                                            .ToListAsync(cancellationToken);
        return new GetUserRoleResponseDto
        {
            Roles = roles
        };
    }
}