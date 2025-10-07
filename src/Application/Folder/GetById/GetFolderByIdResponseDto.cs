namespace Application.Folder.GetById;

public class GetFolderByIdResponseDto
{
    public GetFolderByIdViewModel Folder { get; set; }

    public GetFolderByIdResponseDto(GetFolderByIdViewModel folder)
    {
        Folder = folder;
    }
}