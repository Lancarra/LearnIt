namespace Application.CourseModule.GetByTeacherId;

public class GetByTeacherIdViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }    
    public string? LearnLevel { get; set; }
    public int UserId { get; set; }
    public int DictionaryCount { get; set; }
    public List<GetByTeacherIdStudentsViewModel> Students { get; set; }

}