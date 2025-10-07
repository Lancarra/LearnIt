using Domain.Models.Quiz;
using Infrastructure.Quiz.AnswersResultBuilder;
using MediatR;

namespace Application.Quiz.Answer.CheckAnswer;

public class CheckAnswerRequestDto:IRequest<CheckAnswerResponseDto>
{
    public Guid CardId { get; set; }
    public Guid CardAnswerId { get; set; }

    public ResultBuilderRequest MapToEntity()
    {
        var builderRequest = new ResultBuilderRequest();
        builderRequest.CardId = CardId;
        builderRequest.CardAnswerId = CardAnswerId;
        return builderRequest;
    }
}