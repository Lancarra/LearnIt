using MediatR;

namespace Application.Folder.Update;

public class UpdateFolderRequestDto : IRequest<UpdateFolderResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CourseModuleId { get; set; }
}