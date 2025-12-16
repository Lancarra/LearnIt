using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CourseModule.Update;

public class UpdateModuleHandler : IRequestHandler<UpdateModuleRequestDto, UpdateModuleResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public UpdateModuleHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<UpdateModuleResponseDto> Handle(UpdateModuleRequestDto request, CancellationToken cancellationToken)
    {
        var courseModule = await _context.CourseModules.Include(cm => cm.Students).FirstOrDefaultAsync(cm => cm.Id == request.Id, cancellationToken);
        if (courseModule == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"CourseModule with id {request.Id} does not exist"});
        }

        var courseDublicate =
            await _context.CourseModules.AnyAsync(cm => cm.Name == request.Name && cm.Id != request.Id,
                cancellationToken);
        if (courseDublicate)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Name = $"CourseModule with name {request.Name} already exists"});
        }

        if (request.StudentsAdd.Count > 0)
        {
            var studentsAdd = await _context.Users
                .Where(u => request.StudentsAdd.Contains(u.UserId) && !u.IsDeleted)
                .ToListAsync(cancellationToken);
            if (studentsAdd.Count > 0)
            {
                foreach (var student in studentsAdd)
                {
                    courseModule.Students.Add(student);
                }
            }
        }

        if (request.StudentsRemove.Count > 0)
        {
            var studentsRemove = await _context.Users
                .Where(u => request.StudentsRemove.Contains(u.UserId) && !u.IsDeleted)
                .ToListAsync(cancellationToken);
            if (studentsRemove.Count > 0)
            {
                foreach (var student in studentsRemove)
                {
                    courseModule.Students.Remove(student);
                }
            }

        }
        courseModule.Name = request.Name;
        courseModule.UserId = request.UserId;
        await _context.SaveChangesAsync(cancellationToken);
        
        return new UpdateModuleResponseDto()
        {
            Id = courseModule.Id,
            Name = courseModule.Name,
            UserId = courseModule.UserId,
            Description = courseModule.Description,
            LearnLevel = courseModule.LearnLevel,
            CountStudents =  courseModule.Students.Count,
        };
    }
}