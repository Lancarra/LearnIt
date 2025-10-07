using MediatR;

namespace Application.Folder.GetById;

public class GetFolderByIdRequestDto : IRequest<GetFolderByIdResponseDto>
{
    public Guid FolderId { get; set; }
}