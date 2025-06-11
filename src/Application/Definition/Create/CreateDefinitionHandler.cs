using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Definition.Create;

public class CreateDefinitionHandler : IRequestHandler<CreateDefinitionRequestDto, CreateDefinitionResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public CreateDefinitionHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<CreateDefinitionResponseDto> Handle(CreateDefinitionRequestDto request,
        CancellationToken cancellationToken)
    {
        var dictionaryExists =
            await _context.LearnWordDictionaries.AnyAsync(lwd => lwd.Id == request.DictionaryId, cancellationToken);
        if (!dictionaryExists)
        {
            throw new RestException(HttpStatusCode.BadRequest,
                new { DictionaryId = $"Dictionary with id {request.DictionaryId} does not exist" });
        }
        if (await _context.Definitions.Where(d => d.Word == request.Word).AnyAsync(cancellationToken))
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Word = $"Word with name {request.Word} already exists"});
        }
        
        var definition = new Domain.Models.Definition()
            {
                Id = Guid.NewGuid(),
                Word = request.Word,
                Meaning = request.Meaning,
                BlobURL = request.BlobURL,
                DictionaryId = request.DictionaryId
            };
        var entity = await _context.Definitions.AddAsync(definition, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateDefinitionResponseDto()
            {
                Id = entity.Entity.Id,
                Word = entity.Entity.Word,
                Meaning = entity.Entity.Meaning,
                BlobURL = entity.Entity.BlobURL,
                DictionaryId = entity.Entity.DictionaryId
            };
    }
}