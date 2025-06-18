using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Folder.Create;

public class CreateFolderHandler : IRequestHandler<CreateFolderRequestDto, CreateFolderResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public CreateFolderHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<CreateFolderResponseDto> Handle(CreateFolderRequestDto request, CancellationToken cancellationToken)
    {
        var moduleExists = await _context.CourseModules.AnyAsync(cm => cm.Id == request.CourseModuleId, cancellationToken);
        if (!moduleExists)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {CourseModuleId = $"CourseModule with id {request.CourseModuleId} does not exist"});
        }
        
        if (await _context.Folders.Where(f => f.Name == request.Name).AnyAsync(cancellationToken))
        {
            throw new RestException(HttpStatusCode.BadRequest,
                new { Name = $"Folder with name {request.Name} already exists" });
        }

        var folder = new Domain.Models.Folder()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CourseModuleId = request.CourseModuleId
        };
        var entity = await _context.Folders.AddAsync(folder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateFolderResponseDto()
        {
            Id = entity.Entity.Id,
            Name = entity.Entity.Name,
            CourseModuleId = entity.Entity.CourseModuleId
        };

    }
}