using System.Net;
using Infrastructure.Database;
using Domain.Models;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CourseModule.Create;

public class CreateModuleHandler : IRequestHandler<CreateModuleRequestDto, CreateModuleResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public CreateModuleHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public CreateModuleHandler(LearnContext context)
    {
        _context = context;
    }

    public async Task<CreateModuleResponseDto> Handle(CreateModuleRequestDto request, CancellationToken cancellationToken)
    {
        if (await _context.CourseModules.Where(cm => cm.Name == request.Name).AnyAsync(cancellationToken))
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Name = $"Course with name {request.Name} already exists"});
        }

        var courseModule = new Domain.Models.CourseModule()
        { 
            Id = Guid.NewGuid(),
            Name = request.Name,
            UserId = request.UserId
        };
        var entity = await _context.CourseModules.AddAsync(courseModule, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateModuleResponseDto()
        {
            Id = entity.Entity.Id,
            Name = entity.Entity.Name,
            UserId = entity.Entity.UserId
        };
    }
}