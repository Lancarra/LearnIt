using FluentValidation;

namespace Application.LearnWordDictionary.Update;

public class UpdateDictionaryRequestValidator : AbstractValidator<UpdateDictionaryRequestDto>
{
    public UpdateDictionaryRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.ParentFolderId).NotNull();
    }
}