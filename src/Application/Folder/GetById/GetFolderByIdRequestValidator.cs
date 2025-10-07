using FluentValidation;

namespace Application.Folder.GetById;

public class GetFolderByIdRequestValidator : AbstractValidator<GetFolderByIdRequestDto>
{
    public GetFolderByIdRequestValidator()
    {
        RuleFor(x => x.FolderId).NotEmpty();
    }
}