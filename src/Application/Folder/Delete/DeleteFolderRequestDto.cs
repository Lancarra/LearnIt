using MediatR;

namespace Application.Folder.Delete;

public class DeleteFolderRequestDto : IRequest<DeleteFolderResponseDto>
{
    public Guid Id { get; set; }
}