namespace Application.Users.GetStudents;

public class GetStudentsResponseDto
{
    public List<GetStudentsViewModel>? Students { get; set; }
    public int Count { get; set; }

}