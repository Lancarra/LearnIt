using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Folder.Delete;

public class DeleteFolderHandler
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public DeleteFolderHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<DeleteFolderResponseDto> Handle(DeleteFolderRequestDto request, CancellationToken cancellationToken)
    {
        var folder = await _context.Folders.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (folder == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"Folder with id {request.Id} does not exist"});
        }
        _context.Folders.Remove(folder);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new DeleteFolderResponseDto();
    }
}