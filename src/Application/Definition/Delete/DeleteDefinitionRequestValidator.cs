using FluentValidation;

namespace Application.Definition.Delete;

public class DeleteDefinitionRequestValidator : AbstractValidator<DeleteDefinitionRequestDto>
{
    public DeleteDefinitionRequestValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}