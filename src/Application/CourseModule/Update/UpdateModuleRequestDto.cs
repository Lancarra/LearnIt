using MediatR;

namespace Application.CourseModule.Update;

public class UpdateModuleRequestDto : IRequest<UpdateModuleResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? LearnLevel { get; set; }
    public int UserId { get; set; }
    public List<int>? StudentsAdd { get; set; }
    public List<int>? StudentsRemove { get; set; }
    
}