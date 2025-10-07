using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Folder.Update;

public class UpdateFolderHandler : IRequestHandler<UpdateFolderRequestDto, UpdateFolderResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public UpdateFolderHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<UpdateFolderResponseDto> Handle(UpdateFolderRequestDto request, CancellationToken cancellationToken)
    {
        var courseExists = await _context.CourseModules.AnyAsync(f => f.Id == request.CourseModuleId, cancellationToken);
        if (!courseExists)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {CourseModuleId = $"CourseModule with id {request.CourseModuleId} does not exist"});
        }
        
        var folder = await _context.Folders.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (folder == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"Folder with id {request.Id} does not exist"});
        }
        
        var folderDublicate = await _context.Folders.AnyAsync(f => f.Name == request.Name && f.Id != request.Id, cancellationToken);
        if (folderDublicate)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Name = $"Folder with name {request.Name} already exists"});
        }
        
        folder.Name = request.Name;
        folder.CourseModuleId = request.CourseModuleId;
        _context.Update(folder);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new UpdateFolderResponseDto()
        {
            Id = folder.Id,
            Name = folder.Name,
            CourseModuleId = folder.CourseModuleId
        };
 
    }
}