using FluentValidation;

namespace Application.Quiz.Get;

public class GetQuizRequestValidator : AbstractValidator<GetQuizRequestDto>
{
    public GetQuizRequestValidator()
    {
        RuleFor(x => x.TestCardId).NotEmpty();
    }
}