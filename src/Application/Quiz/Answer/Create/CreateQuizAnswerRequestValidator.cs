using Application.Quiz.Answer.CheckAnswer;
using FluentValidation;

namespace Application.Quiz.Answer.Create;

public class CreateQuizAnswerRequestValidator: AbstractValidator<CreateQuizAnswerRequestDto>
{
    public CreateQuizAnswerRequestValidator()
    {
        RuleFor(x => x.TestCardId).NotNull().NotEmpty();
        RuleFor(x => x.TestUnitAnswers).NotNull().NotEmpty();
    }
}