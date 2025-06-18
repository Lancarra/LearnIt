using Application.Folder.Get;

namespace Application.Folder.GetAll;

public class GetFolderResponseDto
{
    public IEnumerable<GetFolderViewModel> Folders { get; set; }

    public GetFolderResponseDto(List<GetFolderViewModel> folders)
    {
        Folders = folders;
    }
}