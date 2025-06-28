using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Definition.Delete;

public class DeleteDefinitionHandler : IRequestHandler<DeleteDefinitionRequestDto, DeleteDefinitionResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public DeleteDefinitionHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    public async Task<DeleteDefinitionResponseDto> Handle(DeleteDefinitionRequestDto request,
        CancellationToken cancellationToken)
    {
        var definition = await _context.Definitions.FirstOrDefaultAsync(d => d.Id == request.Id);
        if (definition == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"Definition with id {request.Id} does not exist"});
        }
        _context.Definitions.Remove(definition);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new DeleteDefinitionResponseDto(){BlobId = definition.BlobId};
    }
}
