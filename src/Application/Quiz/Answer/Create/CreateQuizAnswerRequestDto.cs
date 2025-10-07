using MediatR;

namespace Application.Quiz.Answer.Create;

public class CreateQuizAnswerRequestDto:IRequest<CreateQuizAnswerResponseDto>
{
    public Guid TestCardId { get; set; }
    public List<CreateQuizAnswerViewModel> TestUnitAnswers { get; set; }
}