namespace Domain.Models;

public class Folder
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<LearnWordDictionary> Dictionaries { get; set; }
    public virtual CourseModule CourseModule { get; set; }
    public Guid CourseModuleId { get; set; }
}