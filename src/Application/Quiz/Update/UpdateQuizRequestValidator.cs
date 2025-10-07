using FluentValidation;

namespace Application.Quiz.Update;

public class UpdateQuizRequestValidator : AbstractValidator<UpdateQuizRequestDto>
{
    public UpdateQuizRequestValidator()
    {
        RuleFor(x => x.TestUnits).NotNull().NotEmpty();
    }
}