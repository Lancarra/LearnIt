using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.LearnWordDictionary.GetAll;

public class GetDictionaryHandler : IRequestHandler<GetDictionaryRequestDto, GetDictionaryResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    
    public GetDictionaryHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<GetDictionaryResponseDto> Handle(GetDictionaryRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var dictionaries = await _context.LearnWordDictionaries.Where(d => d.ParentFolderId == request.ParentFolderId).ToListAsync(cancellationToken);
        PropertyChecker.CheckNullAndThrow404(dictionaries);

        var dictionariesResponse = new List<GetDictionaryViewModel>();
        foreach (var dictionary in dictionaries)
        {
            var dictionaryResponse = new GetDictionaryViewModel()
            {
                Id = dictionary.Id,
                Name = dictionary.Name,
                ParentFolderId = dictionary.ParentFolderId
            };
            dictionariesResponse.Add(dictionaryResponse);
        }
        return new GetDictionaryResponseDto(dictionariesResponse, dictionaries.Count);
    }
}