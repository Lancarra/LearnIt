using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CourseModule.Delete;

public class DeleteModuleHandler : IRequestHandler<DeleteModuleRequestDto, DeleteModuleResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public DeleteModuleHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<DeleteModuleResponseDto> Handle(DeleteModuleRequestDto request, CancellationToken cancellationToken)
    {
        var courseModule = await _context.CourseModules.FirstOrDefaultAsync(cm => cm.Id == request.Id, cancellationToken);

        if (courseModule == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"CourseModule with id {request.Id} does not exist"});
        }
        
        _context.CourseModules.Remove(courseModule);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new DeleteModuleResponseDto();
    }
}