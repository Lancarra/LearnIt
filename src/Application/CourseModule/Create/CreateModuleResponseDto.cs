namespace Application.CourseModule.Create;

public class CreateModuleResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? LearnLevel { get; set; }
    public int UserId { get; set; }
}