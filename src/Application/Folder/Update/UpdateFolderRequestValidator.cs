using FluentValidation;

namespace Application.Folder.Update;

public class UpdateFolderRequestValidator : AbstractValidator<UpdateFolderRequestDto>
{
    public UpdateFolderRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.CourseModuleId).NotNull();
    }
}