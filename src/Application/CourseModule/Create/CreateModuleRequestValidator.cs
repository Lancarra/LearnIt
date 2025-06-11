using FluentValidation;

namespace Application.CourseModule.Create;

public class CreateModuleRequestValidator : AbstractValidator<CreateModuleRequestDto>
{
    public CreateModuleRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}