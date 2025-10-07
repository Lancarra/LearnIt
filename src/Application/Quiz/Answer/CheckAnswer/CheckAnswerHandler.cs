using Infrastructure.CurrentUserAccessor;
using Infrastructure.Quiz;
using Infrastructure.Quiz.AnswersResultBuilder;
using MediatR;

namespace Application.Quiz.Answer.CheckAnswer;

public class CheckAnswerHandler : IRequestHandler<CheckAnswerRequestDto, CheckAnswerResponseDto>
{
    private readonly IResultBuilder _builder;
    private readonly ICurrentUserAccessor _userAccessor;
    
    public CheckAnswerHandler(IResultBuilder builder, ICurrentUserAccessor userAccessor)
    {
        _builder = builder;
        _userAccessor = userAccessor;
    }

    public async Task<CheckAnswerResponseDto> Handle(CheckAnswerRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _builder.GetQuizResult(request.MapToEntity(), cancellationToken );
        var response = new CheckAnswerResponseDto()
        {
            Result = result
        };       
        return response;
    }
}