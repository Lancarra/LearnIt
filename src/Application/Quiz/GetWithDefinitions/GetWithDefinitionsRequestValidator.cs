using FluentValidation;

namespace Application.Quiz.GetWithDefinitions;

public class GetWithDefinitionsRequestValidator : AbstractValidator<GetWithDefinitionsRequestDto>
{
    public GetWithDefinitionsRequestValidator()
    { 
        RuleFor(x => x.TestCardId).NotEmpty();
    }
}