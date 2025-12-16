using Domain.Models;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Quiz.GeneralTest;

public class GeneralTest : IGeneralTest
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    public GeneralTest(LearnContext context,ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }
    public async Task<TestCard> CreateGeneralTest(string name, Guid moduleId, int definitionCount, CancellationToken cancellationToken)
    {
        var tesUnits = new List<TestUnit>();
        var definitions = new List<Definition>();
        var itemCount = 0;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        var testCard = new TestCard();
        testCard.User = user;
        testCard.UserId = user.UserId;
        if (!string.IsNullOrWhiteSpace(name))
        {
            testCard.Name = name;
        }
        var module = await _context.CourseModules.Include(cm => cm.Folders).ThenInclude(f => f.Dictionaries).ThenInclude(d => d.Definitions).FirstOrDefaultAsync(cm => cm.Id == moduleId, cancellationToken);
        var modules = module.Folders.SelectMany(f => f.Dictionaries);
        foreach (var dictionary in modules)
        {
            definitions.AddRange(dictionary.Definitions);
        }
        var maxDistractors = Math.Min(3, definitions.Count - 1);
        testCard.DictionaryId = definitions.First().DictionaryId;
        foreach (var definition in definitions)
        {
            if (itemCount <= definitionCount)
            {
                var AdditionalMeaning = new List<string>();
                AdditionalMeaning.Add(definition.Meaning);
                
                if (definitions.Count > 1)
                {
                    var three = definitions.Where(d => d.Id != definition.Id).OrderBy(_ => Random.Shared.Next()).Take(maxDistractors).Select(d => d.Meaning).ToList();
                
                    AdditionalMeaning.AddRange(three);
                }
                AdditionalMeaning = AdditionalMeaning.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(_ => Random.Shared.Next()).ToList();
                
                var testUnit = new TestUnit
                {
                    AdditionalAnswers = AdditionalMeaning,
                    Definition = definition,
                    DefinitionId = definition.Id,
                    TestCard = testCard,
                    TestCardId = testCard.Id,
                };
                tesUnits.Add(testUnit);
            
                itemCount += 1;
            }
            else
            {
                break;
            }
        }
        testCard.TestUnits = tesUnits;
        await _context.AddAsync(testCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return testCard;
    }
}
