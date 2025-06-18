using FluentValidation;

namespace Application.CourseModule.Update;

public class UpdateModuleRequestValidator : AbstractValidator<UpdateModuleRequestDto>
{
    public UpdateModuleRequestValidator()
    {
        RuleFor(x => x.Name).Must(x => x.Length <= 100);
    }
}