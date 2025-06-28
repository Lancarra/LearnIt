using FluentValidation;

namespace Application.Definition.GetById;

public class GetDefinitionByIdRequestValidator: AbstractValidator<GetDefinitionByIdRequestDto>
{
    public GetDefinitionByIdRequestValidator()
    {
        RuleFor(x => x.DefinitionId).NotNull().NotEmpty();
    }
}