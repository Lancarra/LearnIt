using System.Net;
using Application.Users.GetTeachers;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetStudents;

public class GetStudentsHandler : IRequestHandler<GetStudentsRequestDto, GetStudentsResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetStudentsHandler(LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<GetStudentsResponseDto> Handle(GetStudentsRequestDto request, CancellationToken cancellationToken)
    {
        var permissions = _userAccessor.GetCurrentRoles(); 

        if (permissions == null || !permissions.Contains("Admin"))
        {
            throw new RestException(HttpStatusCode.NotFound,
                new { User = "User does not have permission to access this resource" });
        }
        var role = await  _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Student");
        
        var response = new GetStudentsResponseDto();
        response.Students = new List<GetStudentsViewModel>();

    
        var students = await _context.Users.Include(u => u.Achievement)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted && u.UserRoles.Any(ur => ur.RoleId == role.RoleId))
            .ToListAsync(cancellationToken);

        foreach (var student in students)
        {
            response.Students.Add(new GetStudentsViewModel
            {
                UserId = student.UserId,
                Email = student.Email,
                Username = student.Username,
                RoleName = string.Join(",", student.UserRoles.Select(ur => ur.Role.RoleName)),
                Achievement = student.Achievement?.Name,
                BlobId = student.BlobId
            });
        }
        response.Count = response.Students.Count;
        return response;
    }
}