using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Definition.Update;

public class UpdateDefinitionHandler : IRequestHandler<UpdateDefinitionRequestDto, UpdateDefinitionResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public UpdateDefinitionHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    
    public async Task<UpdateDefinitionResponseDto> Handle(UpdateDefinitionRequestDto request, CancellationToken cancellationToken)
    {
        var dictionaryExists = await _context.LearnWordDictionaries.AnyAsync(lwd => lwd.Id == request.DictionaryId, cancellationToken);
        if (!dictionaryExists)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {DictionaryId = $"Dictionary with id {request.DictionaryId} does not exist"});
        }
        
        var definition = await _context.Definitions.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
        if (definition == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"Definition with id {request.Id} does not exist"});
        }
        
        var definitionDublicate = await _context.Definitions.AnyAsync(d => d.Word == request.Word && d.Id != request.Id, cancellationToken);
        if (definitionDublicate)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Word = $"Word with name {request.Word} already exists"});
        }
        
        definition.Word = request.Word;
        definition.Meaning = request.Meaning;
        definition.BlobURL = request.BlobURL;
        definition.DictionaryId = request.DictionaryId;
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateDefinitionResponseDto()
        {
            Id = definition.Id,
            Word = definition.Word,
            Meaning = definition.Meaning,
            BlobURL = definition.BlobURL,
            DictionaryId = definition.DictionaryId
        };
    }
}