using FluentValidation;

namespace Application.Quiz;

public class CreateQuizRequestValidator: AbstractValidator<CreateQuizRequestDto>
{
    public CreateQuizRequestValidator()
    {
        RuleFor(x => x.TestUnits).NotNull().NotEmpty();
    }
}
