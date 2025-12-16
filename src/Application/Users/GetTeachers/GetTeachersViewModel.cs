namespace Application.Users.GetTeachers;

public class GetTeachersViewModel
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string? RoleName { get; set; }
    public string? Achievement {get; set;}
    public Guid? BlobId { get; set; }
}