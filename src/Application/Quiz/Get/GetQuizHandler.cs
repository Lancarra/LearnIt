using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quiz.Get;

public class GetQuizHandler : IRequestHandler<GetQuizRequestDto, GetQuizResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetQuizHandler(LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<GetQuizResponseDto> Handle(GetQuizRequestDto request, CancellationToken cancellationToken)
    {
        var roles = _userAccessor.GetCurrentRoles();
        PropertyChecker.CheckNullAndThrow404(roles);
        
        if (!roles.Contains("Admin") && !roles.Contains("Teacher"))
        {
            throw new RestException(HttpStatusCode.Unauthorized, new {Message = "You are haven't permission to perform this action"});
        }
        
        var card = await _context.TestCards.Include(tc => tc.TestUnits).FirstOrDefaultAsync(c => c.Id == request.TestCardId, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(card);

        var response = new GetQuizResponseDto
        {
            CardId = request.TestCardId,
            TestUnits = new List<GetQuizViewModel>()
        };

        foreach (var u in card.TestUnits)
        {
            response.TestUnits.Add(new GetQuizViewModel
            {
                TestUnitId = u.Id,
                AdditionalAnswers = Shuffle(u.AdditionalAnswers),
                DictionaryId = u.DictionaryId ,
                DefinitionId = u.DefinitionId,
                TestCardId = u.TestCardId
            });
        }
        return response;
    }

    private static List<string> Shuffle(List<string> list)
    {
        var rng = new Random();
        var shuffled = list.ToList();
        int n = shuffled.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (shuffled[n], shuffled[k]) = (shuffled[k], shuffled[n]);
        }
        return shuffled;
    }
}