using FluentValidation;

namespace Application.Quiz.ResultByDictionaryKakoito;

public class ResultByDictionaryRequestValidator : AbstractValidator<ResultByDictionaryRequestDto>
{
    public ResultByDictionaryRequestValidator()
    {
        RuleFor(x => x.DictionaryId).NotNull().NotEmpty();
        RuleFor(x => x.TeacherId).NotEmpty().GreaterThan(0);
    }
}