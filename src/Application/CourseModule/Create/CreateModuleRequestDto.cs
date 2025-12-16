using MediatR;

namespace Application.CourseModule.Create;

public class CreateModuleRequestDto : IRequest<CreateModuleResponseDto>
{
    public string Name { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public string? LearnLevel { get; set; }
}