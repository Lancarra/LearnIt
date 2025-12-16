using System.Net;
using Application.Users.GetAllStudentsByTeacherId;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetTeachers;

public class GetTeachersHandler : IRequestHandler<GetTeachersRequestDto, GetTeachersResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetTeachersHandler(LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<GetTeachersResponseDto> Handle(GetTeachersRequestDto request, CancellationToken cancellationToken)
    {
        var permissions = _userAccessor.GetCurrentRoles(); 

        if (permissions == null || !permissions.Contains("Admin"))
        {
            throw new RestException(HttpStatusCode.NotFound,
                new { User = "User does not have permission to access this resource" });
        }
        var role = await  _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Teacher");
        
        var responce = new GetTeachersResponseDto();
        responce.Teachers = new List<GetTeachersViewModel>();

    
        var teachers = await _context.Users.Include(u => u.Achievement)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted && u.UserRoles.Any(ur => ur.RoleId == role.RoleId))
            .ToListAsync(cancellationToken);

        foreach (var teacher in teachers)
        {
            responce.Teachers.Add(new GetTeachersViewModel
            {
                UserId = teacher.UserId,
                Email = teacher.Email,
                Username = teacher.Username,
                RoleName = string.Join(",", teacher.UserRoles.Select(ur => ur.Role.RoleName)),
                Achievement = teacher.Achievement?.Name,
                BlobId = teacher.BlobId
            });
        }
        responce.Count = responce.Teachers.Count;
        return responce;
    }
}