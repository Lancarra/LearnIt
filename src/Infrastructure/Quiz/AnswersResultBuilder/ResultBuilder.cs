using Domain.Models.Quiz;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Quiz.AnswersResultBuilder;

public class ResultBuilder : IResultBuilder
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    public ResultBuilder(LearnContext context,ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }
    
    public async Task<ResultBuilderModel> GetQuizResult(ResultBuilderRequest request, CancellationToken cancellationToken)
    {
        var correctAnswer = 0;
        var incorrectAnswer = 0;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var testCard = await _context.TestCards.Include(tc => tc.TestUnits).ThenInclude(tu => tu.Definition)
                                                       .FirstOrDefaultAsync(c => c.Id == request.CardId, cancellationToken);
        var testCardAnswer = await _context.TestCardAnswer.Include(tca => tca.TestUnitAnswers)
            .FirstOrDefaultAsync(ca => ca.Id == request.CardAnswerId, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(testCardAnswer);
        
        var testCardUnits = testCard.TestUnits.Count();
        incorrectAnswer = testCardUnits - testCardAnswer.TestUnitAnswers.Count();

        var achievements = await _context.Achievements.ToListAsync(cancellationToken);
        PropertyChecker.CheckNullAndThrow404(achievements);
        
        foreach (var answer in testCardAnswer.TestUnitAnswers)
        {
            var testUnit =  testCard.TestUnits.FirstOrDefault(t => t.Id == answer.TestUnitId);
            if (testUnit.Definition.Meaning.ToLower() == answer.Answer.ToLower())
            {
                correctAnswer++;
            }
            else
            {
                incorrectAnswer++;
            }
        }
        var achievementDescription = string.Empty;
        if (user?.AchievementId == null)
        {
            achievementDescription = "Your current level start";
        }
        else
        { 
            var achievementName = achievements.FirstOrDefault(a => a.Id == user?.AchievementId).Name;
            achievementDescription = $"Your current level {achievementName}";
        }
        
        switch (user.Rating)
        {
            case 5:
                achievementDescription = $"Your current level Beginner. Next level: Advanced ";
               break;
           
           case 10:
                achievementDescription = $"Your current level Advanced. Next level: Professional ";
               break;
           
           case 15:
                achievementDescription = $"Your current level Professional. You are Cool ";
               break;
           default:
                achievementDescription = $"You need passed test for getting reting";
                break;
        }

        var result = new ResultBuilderModel
        {
            CorrectAnswers = correctAnswer,
            IncorrectAnswers = incorrectAnswer,
            CardId = request.CardId,
            AchievementDescription = achievementDescription
        };
        
        return result;
    }
}