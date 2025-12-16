using System.Net;
using Domain.Models;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.UpdatePermission;

public class UpdatePermissionHandler : IRequestHandler<UpdatePermissionRequestDto, UpdatePermissionResponseDto>
{
    private readonly LearnContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public UpdatePermissionHandler(LearnContext context, IPasswordHasher passwordHasher, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _currentUserAccessor = currentUserAccessor;
    }
    public async Task<UpdatePermissionResponseDto> Handle(UpdatePermissionRequestDto request, CancellationToken cancellationToken)
    {
        #region For any handler with admin permission //TODO

        var user = await _context.Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Email == _currentUserAccessor.GetCurrentEmail() 
                                      && !x.IsDeleted, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var isAdmin = user.UserRoles.Any(ur => ur.Role.RoleName == "Admin");
        
        if (!isAdmin)
        {
            PropertyChecker.CheckRoleAndThrow403();
            //throw new UnauthorizedAccessException("You don't have permission to update roles");       
        }
        #endregion
        

        if (!request.DeleteRole)
        {
            var updateUser = await _context.Users.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            PropertyChecker.CheckNullAndThrow404(updateUser);

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == request.RoleId);
            PropertyChecker.CheckNullAndThrow404(role);

            var userRole = new UserRole()
            {
                RoleId = request.RoleId,
                Role = role,
                User = updateUser,
                UserId = updateUser.UserId,
            };
            await _context.UserRole.AddAsync(userRole);

            var permissionRequest = await _context.PermissionRequests.FirstOrDefaultAsync(pr =>
                pr.RoleName == role.RoleName && pr.RequestUserId == request.UserId);
            if (permissionRequest != null)
            {
                _context.PermissionRequests.Remove(permissionRequest);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return new UpdatePermissionResponseDto(){Result= $"Role {role.RoleName} add to user {updateUser.Username} successfully"};
        }
        else
        {
            var updateUser = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(x => x.UserId == request.UserId);
            PropertyChecker.CheckNullAndThrow404(updateUser);
            if (!updateUser.UserRoles.Any(ur => ur.RoleId == request.RoleId))
            {
                throw new RestException(HttpStatusCode.BadRequest,  new string($"User {updateUser.Username} doesn't have role {request.RoleId}"));
            }
            var role = updateUser.UserRoles.FirstOrDefault(ur => ur.RoleId == request.RoleId);
            PropertyChecker.CheckNullAndThrow404(role);
            
            _context.UserRole.Remove(role);
            await _context.SaveChangesAsync(cancellationToken);
            return new UpdatePermissionResponseDto(){Result= $"Role {role.Role.RoleName} remove user {updateUser.Username} successfully"};
        }
    }
}
