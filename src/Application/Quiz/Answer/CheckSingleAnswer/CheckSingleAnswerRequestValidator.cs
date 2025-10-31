using Application.Quiz.Answer.Create;
using FluentValidation;

namespace Application.Quiz.Answer.CheckSingleAnswer;

public class CheckSingleAnswerRequestValidator: AbstractValidator<CheckSingleAnswerRequestDto>
{
    public CheckSingleAnswerRequestValidator()
    {
        RuleFor(x => x.DefinitionId).NotNull().NotEmpty();
        RuleFor(x => x.UserAnswer).NotNull().NotEmpty();
    }
}