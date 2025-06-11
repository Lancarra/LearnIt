namespace Application.Users.Login
{
    public class LoginUserResponseDto
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
        public string LearnItToken { get; set; }

        public DateTime TokenValidTo { get; set; }
        public bool Has2FAuthEnabled { get; set; }
    }
}
