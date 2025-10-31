using Application.Definition.Get;
using Application.Definition.GetAll;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quiz.GetCards;

public class GetAllTestCardByDictionaryIdHandler : IRequestHandler<GetAllTestCardByDictionaryIdRequestDto, GetAllTestCardByDictionaryIdResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetAllTestCardByDictionaryIdHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _context = context;
        _userAccessor = userAccessor;
    }
    public async Task<GetAllTestCardByDictionaryIdResponseDto> Handle(GetAllTestCardByDictionaryIdRequestDto request, CancellationToken cancellationToken)
    {
        var result = new GetAllTestCardByDictionaryIdResponseDto();
        result.TestCards = new List<GetAllTestCardByDictionaryIdViewModel>();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var cards = await _context.TestCards.Where(d => d.DictionaryId == request.DictionaryId).ToListAsync(cancellationToken);
        PropertyChecker.CheckNullAndThrow404(cards);
        
        foreach (var card in cards)
        {
            result.TestCards.Add(new GetAllTestCardByDictionaryIdViewModel()
            {
                CardId = card.Id,
                Name = card.Name,
            });
        }
        return result;
    }
}