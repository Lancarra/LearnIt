using FluentValidation;

namespace Application.Quiz.Answer.CheckAnswer;

public class CheckAnswerRequestValidator: AbstractValidator<CheckAnswerRequestDto>
{
    public CheckAnswerRequestValidator()
    {
        RuleFor(x => x.CardAnswerId).NotNull().NotEmpty();
        RuleFor(x => x.CardId).NotNull().NotEmpty();
    }
}