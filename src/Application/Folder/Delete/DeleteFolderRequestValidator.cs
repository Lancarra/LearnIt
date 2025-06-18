using FluentValidation;

namespace Application.Folder.Delete;

public class DeleteFolderRequestValidator : AbstractValidator<DeleteFolderRequestDto>
{
    public DeleteFolderRequestValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}