using MediatR;

namespace Application.Quiz.Answer.CheckSingleAnswer;

public class CheckSingleAnswerRequestDto:IRequest<CheckSingleAnswerResponseDto>
{
    public Guid DefinitionId { get; set; }
    public string UserAnswer { get; set; }
}