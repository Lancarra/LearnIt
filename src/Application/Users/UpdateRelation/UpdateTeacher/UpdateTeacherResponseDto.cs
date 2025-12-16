namespace Application.Users.UpdateRelation.UpdateTeacher;

public class UpdateTeacherResponseDto
{
    public int UserId { get; set; }
    public List <int?> StudentsId { get; set; }
}