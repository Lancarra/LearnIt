namespace Application.Users.Read
{
    public class ReadUserResponseDTO
    {
        public string Username { get; set; }

        public int UserId { get; set; }


        public string Email { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }

        public string Token { get; set; }

        public string Language { get; set; }

        public int CompanyId { get; set; }
        public bool IsUserSuperAdmin { get; set; }
        public bool IsUserLocalAdmin { get; set; }
        public DateTime TokenValidTo { get; set; }
        public bool Has2FAuthEnabled { get; set; }
    }
}
