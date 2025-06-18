using FluentValidation;

namespace Application.Folder.Create;

public class CreateFolderRequestValidator : AbstractValidator<CreateFolderRequestDto>
{
    public CreateFolderRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.CourseModuleId).NotNull();
    }
    
}