namespace Application.Users.Update;

public class UpdateUserResponseDto 
{
    public string Email { get; set; }

    public string Token { get; set; }

    public DateTime TokenValidTo { get; set; }
}