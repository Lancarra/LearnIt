using System.Net;
using Domain;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetAllStudentsByTeacherId;

public class GetAllStudentsByTeacherIdHandler : IRequestHandler<GetAllStudentsByTeacherIdRequestDto, GetAllStudentsByTeacherIdResponseDto>

{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetAllStudentsByTeacherIdHandler(LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }
        
   
    public async Task<GetAllStudentsByTeacherIdResponseDto> Handle(GetAllStudentsByTeacherIdRequestDto request, CancellationToken cancellationToken)
    {
        var permissions = _userAccessor.GetCurrentRoles(); 

        if (permissions == null || !permissions.Contains("Admin") && !permissions.Contains("Teacher"))
        {
            throw new RestException(HttpStatusCode.NotFound,
                new { User = "User does not have permission to access this resource" });
        }
    
        var teacher = await _context.Users.Where(x => x.UserId == request.TeacherId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        if (teacher == null)
            throw new RestException(HttpStatusCode.NotFound, new { Teacher = "Teacher not found" });
        var responce = new GetAllStudentsByTeacherIdResponseDto();
        responce.Students = new List<GetUsersByTeacherIdViewModel>();

    
        var students = await _context.Users.Include(u => u.Achievement)
                                                    .Include(u => u.UserRoles)
                                                    .ThenInclude(ur => ur.Role)
                                                    .Where(u => teacher.StudentsId.Contains(u.UserId) && !u.IsDeleted)
                                                    .ToListAsync(cancellationToken);

        foreach (var student in students)
        {
            responce.Students.Add(new GetUsersByTeacherIdViewModel
            {
                UserId = student.UserId,
                Email = student.Email,
                Username = student.Username,
                RoleName = string.Join(",", student.UserRoles.Select(ur => ur.Role.RoleName)),
                Achievement = student.Achievement?.Name,
                BlobId = student.BlobId
            });
        }

        return responce;
    }
}