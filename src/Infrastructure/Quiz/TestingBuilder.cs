using Domain.Models;
using Domain.Models.Quiz;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Quiz;

public class TestingBuilder : ITestingBuilder
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    public TestingBuilder(LearnContext context,ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }
    public async Task<TestCard> CreateQuiz(string name, List<TestUnitRequestDto> request, CancellationToken cancellationToken)
    {
        var tesUnits = new List<TestUnit>();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        var testCard = new TestCard();
        testCard.User = user;
        testCard.UserId = user.UserId;
        if (!string.IsNullOrWhiteSpace(name))
        {
            testCard.Name = name;
        }
        var definitions = await _context.Definitions.Where(d => d.DictionaryId == request.First().DictionaryId).ToListAsync(cancellationToken);
        var maxDistractors = Math.Min(3, definitions.Count - 1);
        testCard.DictionaryId = definitions.First().DictionaryId;
        foreach (var requestUnit in request)
        {
            var result = definitions.FirstOrDefault(d => d.Id == requestUnit.DefinitionId);
            if (result != null)
            {
                var AdditionalMeaning = new List<string>();
                AdditionalMeaning.Add(result.Meaning);
                
                if (definitions.Count > 1)
                {
                    var three = definitions.Where(d => d.Id != result.Id).OrderBy(_ => Random.Shared.Next()).Take(maxDistractors).Select(d => d.Meaning).ToList();
                
                    AdditionalMeaning.AddRange(three);
                }
                AdditionalMeaning = AdditionalMeaning.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(_ => Random.Shared.Next()).ToList();
                
                var testUnit = new TestUnit
                {
                    AdditionalAnswers = AdditionalMeaning,
                    Definition = result,
                    DefinitionId = result.Id,
                    TestCard = testCard,
                    TestCardId = testCard.Id,
                };
                tesUnits.Add(testUnit);
            }
        }
        testCard.TestUnits = tesUnits;
        await _context.AddAsync(testCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return testCard;
    }

    public async Task<TestCard> UpdateQuiz(Guid cardId, string name, List<UpdateTestUnitViewModel> testUnits, CancellationToken cancellationToken)
    {
        var card = await _context.TestCards.Include(tc => tc.TestUnits).FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(card);
        if (testUnits.Count == 0)
            return card;
        if (!string.IsNullOrWhiteSpace(name))
        {
            card.Name = name;
        }
        var dtoById = testUnits.ToDictionary(t => t.Id);
        foreach (var unit in card.TestUnits)
        {
            if (dtoById.TryGetValue(unit.Id, out var dto))
            {
                unit.AdditionalAnswers = dto.AdditionalAnswers;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return card;
    }
}