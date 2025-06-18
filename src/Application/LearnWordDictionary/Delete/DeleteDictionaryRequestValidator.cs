using FluentValidation;

namespace Application.LearnWordDictionary.Delete;

public class DeleteDictionaryRequestValidator : AbstractValidator<DeleteDictionaryRequestDto>
{
    public DeleteDictionaryRequestValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}