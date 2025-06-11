using FluentValidation;

namespace Application.Definition.Update;

public class UpdateDefinitionRequestValidator : AbstractValidator<UpdateDefinitionRequestDto>
{
    public UpdateDefinitionRequestValidator()
    {
        RuleFor(x => x.Word).NotNull().NotEmpty();
        RuleFor(x => x.Meaning).NotNull().NotEmpty();
    }
}