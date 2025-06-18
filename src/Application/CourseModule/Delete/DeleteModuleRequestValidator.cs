using FluentValidation;

namespace Application.CourseModule.Delete;

public class DeleteModuleRequestValidator : AbstractValidator<DeleteModuleRequestDto>
{
    public DeleteModuleRequestValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}