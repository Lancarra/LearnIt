using MediatR;

namespace Application.Folder.GetAll;

public class GetFolderRequestDto : IRequest<GetFolderResponseDto>
{
    public Guid CourseModuleId { get; set; }

}