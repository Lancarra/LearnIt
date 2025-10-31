using Application.Quiz.Answer.CheckAnswer;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Quiz.AnswersResultBuilder;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quiz.Answer.CheckSingleAnswer;

public class CheckSingleAnswerHandler: IRequestHandler<CheckSingleAnswerRequestDto, CheckSingleAnswerResponseDto>
{
    private readonly IResultBuilder _builder;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly LearnContext _context;
    
    public CheckSingleAnswerHandler(IResultBuilder builder, ICurrentUserAccessor userAccessor, LearnContext context)
    {
        _builder = builder;
        _userAccessor = userAccessor;
        _context = context;
    }

    public async Task<CheckSingleAnswerResponseDto> Handle(CheckSingleAnswerRequestDto request, CancellationToken cancellationToken)
    {
        var definition = await _context.Definitions.FirstOrDefaultAsync(d => d.Id == request.DefinitionId,
                                                                        cancellationToken);
        PropertyChecker.CheckNullAndThrow404(definition);
        
        var result = String.Equals(request.UserAnswer, definition?.Meaning);
        
        var response = new CheckSingleAnswerResponseDto
        {
            UserAnswer = request.UserAnswer,
            IsCorrect = result,
            DefinitionWord = definition.Word,
            DefinitionMeaning = definition.Meaning,
        };
        return response;
    }
}