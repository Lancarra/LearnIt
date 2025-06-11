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
        var courseModule = await _context.CourseModules.FirstOrDefaultAsync(cm => cm.Id == request.Id, cancellationToken);
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

        courseModule.Name = request.Name;
        courseModule.UserId = request.UserId;
        await _context.SaveChangesAsync(cancellationToken);
        
        return new UpdateModuleResponseDto()
        {
            Id = courseModule.Id,
            Name = courseModule.Name,
            UserId = courseModule.UserId
        };
    }
}