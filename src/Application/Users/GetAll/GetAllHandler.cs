using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetAll;

public class GetAllHandler : IRequestHandler<GetAllRequestDto, GetAllResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    
    public GetAllHandler(LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }
    public async Task<GetAllResponseDto> Handle(GetAllRequestDto request, CancellationToken cancellationToken)
    {
        if (!_userAccessor.GetCurrentRoles().Contains("Admin"))
        {
            throw new RestException(HttpStatusCode.NotFound, new { User = "User not have permission to access this resource" });
        }
        var response = new GetAllResponseDto();
        response.Users = new List<GetAllUserViewModel>();
        var users = await _context.Users.Include(u => u.Achievement)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted).ToListAsync(cancellationToken);
        
        if (users.Count < 1)
        {
            throw new RestException(HttpStatusCode.NotFound, new { Users = "Users not found" });
        }

        foreach (var user in users)
        {
                response.Users.Add(new GetAllUserViewModel
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Username = user.Username,
                    RoleName = string.Join(",", user.UserRoles.Select(ur => ur.Role.RoleName)),
                    Achievement = user.Achievement?.Name,
                    BlobId = user.BlobId
                });
        }

        response.Users = response.Users.OrderBy(u => u.RoleName).ToList();
        return response;
    }
}