using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.LearnWordDictionary.Create;

public class CreateDictionaryHandler : IRequestHandler<CreateDictionaryRequestDto, CreateDictionaryResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public CreateDictionaryHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    
    public async Task<CreateDictionaryResponseDto> Handle(CreateDictionaryRequestDto request, CancellationToken cancellationToken)
    {
        var folderExists = await _context.Folders.AnyAsync(f => f.Id == request.ParentFolderId, cancellationToken);
        if (!folderExists)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {ParentFolderId = $"Folder with id {request.ParentFolderId} does not exist"});
        }

        if (await _context.LearnWordDictionaries.Where(lwd => lwd.Name == request.Name).AnyAsync(cancellationToken))
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Name = $"Dictionary with name {request.Name} already exists"});
        }

        var dictionary = new Domain.Models.LearnWordDictionary()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ParentFolderId = request.ParentFolderId
        };
        var entity = await _context.LearnWordDictionaries.AddAsync(dictionary, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new CreateDictionaryResponseDto()
        {
            Id = entity.Entity.Id,
            Name = entity.Entity.Name,
            ParentFolderId = entity.Entity.ParentFolderId
        };
    }
}