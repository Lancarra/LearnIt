using FluentValidation;

namespace Application.LearnWordDictionary.Create;

public class CreateDictionaryRequestValidator : AbstractValidator<CreateDictionaryRequestDto>
{
    public CreateDictionaryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.ParentFolderId).NotNull();
    }
}