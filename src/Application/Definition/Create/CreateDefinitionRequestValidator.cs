using FluentValidation;

namespace Application.Definition.Create;

public class CreateDefinitionRequestValidator: AbstractValidator<CreateDefinitionRequestDto>
{
    public CreateDefinitionRequestValidator()
    {
        RuleFor(x => x.Word).NotNull().NotEmpty();
        RuleFor(x => x.Meaning).NotNull().NotEmpty();
    }
}   