namespace Domain.Models;

public class CourseModule
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? LearnLevel { get; set; }
    public virtual User User { get; set; }
    public int UserId { get; set; }
    public virtual ICollection<User> Students { get; set; } = new List<User>();
    public virtual ICollection<Folder>? Folders { get; set; }
    
}