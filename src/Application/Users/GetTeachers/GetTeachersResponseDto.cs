namespace Application.Users.GetTeachers;

public class GetTeachersResponseDto
{
    public List<GetTeachersViewModel>? Teachers { get; set; }
    public int Count { get; set; }
}