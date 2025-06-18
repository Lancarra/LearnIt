using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.LearnWordDictionary.Delete;

public class DeleteDictionaryHandler : IRequestHandler<DeleteDictionaryRequestDto, DeleteDictionaryResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public DeleteDictionaryHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<DeleteDictionaryResponseDto> Handle(DeleteDictionaryRequestDto request, CancellationToken cancellationToken)
    {
        var dictionary = await _context.LearnWordDictionaries.FirstOrDefaultAsync(lwd => lwd.Id == request.Id);
        if (dictionary == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"Dictionary with id {request.Id} does not exist"});
        }
        _context.LearnWordDictionaries.Remove(dictionary);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new DeleteDictionaryResponseDto();
    }
}