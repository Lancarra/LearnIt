namespace Domain.Models;

public class CourseModule
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual User User { get; set; }
    public int UserId { get; set; }
    public virtual ICollection<Folder> Folders { get; set; }
}