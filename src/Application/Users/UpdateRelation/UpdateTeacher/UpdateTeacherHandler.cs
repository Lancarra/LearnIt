using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.UpdateRelation.UpdateTeacher;

public class UpdateTeacherHandler : IRequestHandler<UpdateTeacherRequestDto, UpdateTeacherResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    
    public UpdateTeacherHandler (LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<UpdateTeacherResponseDto> Handle(UpdateTeacherRequestDto request,
        CancellationToken cancellationToken)
    {
        var permissions = _userAccessor.GetCurrentRoles(); 

        if (permissions == null || !permissions.Contains("Admin") && !permissions.Contains("Teacher"))
        {
            throw new RestException(HttpStatusCode.NotFound,
                new { User = "User does not have permission to access this resource" });
        }


        var teacher = await _context.Users.Where(x => x.UserId == request.TeacherId && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        switch (request.Operation)
        {
            case (Operations.Add):
            {
                if (request.StudentsId?.Count > 0)
                {
                    foreach (var addStudent in request.StudentsId)
                    {
                        var student = await _context.Users.Where(x => x.UserId == addStudent && !x.IsDeleted)
                            .FirstOrDefaultAsync(cancellationToken);
                        student.StudentsId ??= new List<int?>();
                        if (student.TeachersId.Contains(request.TeacherId))
                        {
                            throw new RestException(HttpStatusCode.BadRequest, new { Error = "You don't add this teacher, so sit and cry, bedosia!" });
                        }
                        student.TeachersId.Add(request.TeacherId);
                        teacher.StudentsId ??= new List<int?>(); 
                        if (!teacher.StudentsId.Contains(addStudent))
                            teacher.StudentsId.Add(addStudent);
                    }
                }

                break;
            }

            case Operations.Remove:
            {
                if (request.StudentsId?.Count > 0 && teacher.StudentsId != null)
                {
                    foreach (var removeStudent in request.StudentsId)
                    {
                        var student = await _context.Users.Where(x => x.UserId == removeStudent && !x.IsDeleted)
                            .FirstOrDefaultAsync(cancellationToken);
                        student.TeachersId.Remove(request.TeacherId);
                        
                        if (teacher.StudentsId.Contains(removeStudent))
                            teacher.StudentsId.Remove(removeStudent);
                    }
                }

                break;
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
        return new UpdateTeacherResponseDto
        {
            UserId = teacher.UserId,
            StudentsId = teacher.StudentsId
        };
    }
}