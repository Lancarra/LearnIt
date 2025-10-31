using FluentValidation;

namespace Application.Quiz;

public class CreateQuizRequestValidator: AbstractValidator<CreateQuizRequestDto>
{
    public CreateQuizRequestValidator()
    {
        RuleFor(x => x.DictionaryId).NotNull().NotEmpty();
        RuleFor(x => x.QuestionsCount).NotNull().NotEmpty();
    }
}
