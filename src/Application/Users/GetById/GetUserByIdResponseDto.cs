namespace Application.Users.GetById;

public class GetUserByIdResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string RoleName { get; set; }
    public string Achievement {get; set;}
    public Guid? BlobId { get; set; }
    public List <int?> TeachersId { get; set; }
    public List <int?> StudentsId { get; set; }
}