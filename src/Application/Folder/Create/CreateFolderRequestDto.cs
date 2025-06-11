using MediatR;

namespace Application.Folder.Create;

public class CreateFolderRequestDto : IRequest<CreateFolderResponseDto>
{
    public string Name { get; set; }
    public Guid CourseModuleId { get; set; }
}