using Domain.Models.Quiz;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Quiz.AnswersResultBuilder;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quiz.Answer.Create;

public class CreateQuizAnswerHandler: IRequestHandler<CreateQuizAnswerRequestDto, CreateQuizAnswerResponseDto>
{
    private readonly IResultBuilder _builder;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly LearnContext _context;
    
    public CreateQuizAnswerHandler(IResultBuilder builder, ICurrentUserAccessor userAccessor,  LearnContext context)
    {
        _builder = builder;
        _userAccessor = userAccessor;
        _context = context;
    }

    public async Task<CreateQuizAnswerResponseDto> Handle(CreateQuizAnswerRequestDto request, CancellationToken cancellationToken)
    {
        var answers = new List<TestUnitAnswers>();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail());
        PropertyChecker.CheckNullAndThrow404(user);

        var cardAnswer = new TestCardAnswer()
        {
            TestCardId = request.TestCardId,
            TestUnitAnswers = answers,
            User = user,
            UserId = user.UserId
        };

        foreach (var answer in request.TestUnitAnswers)
        {
            var unitAnswer = new TestUnitAnswers()
            {
                Answer = answer.Answer,
                TestUnitId = answer.TestUnitId,
                TestCardAnswerId = cardAnswer.Id
            };
            answers.Add(unitAnswer);
        }
        cardAnswer.TestUnitAnswers = answers;
        
        await _context.TestCardAnswer.AddAsync(cardAnswer, cancellationToken);
        await _context.TestUnitAnswers.AddRangeAsync(answers, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var checkRequest = new ResultBuilderRequest
        {
            CardId = request.TestCardId,
            CardAnswerId = cardAnswer.Id
        };
        var result = await _builder.GetQuizResult(checkRequest, cancellationToken );
        var response = new CreateQuizAnswerResponseDto()
        {
            Result = result
        };       
        return response;
    }
}