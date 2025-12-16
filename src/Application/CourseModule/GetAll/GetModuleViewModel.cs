namespace Application.CourseModule.GetAll;

public class GetModuleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public string Author { get; set; }
    public string? Description { get; set; }
    public string? LearnLevel { get; set; }
    
}