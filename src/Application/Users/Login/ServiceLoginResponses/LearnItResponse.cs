namespace Application.Users.Login.ServiceLoginResponses
{
    public class LearnItResponse
    {
        public class UserContainer
        {
            public string Username { get; set; }
            public int UserId { get; set; }
            public string Email { get; set; }
            public string Bio { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Image { get; set; }
            public string Token { get; set; }
            public string LearnItToken { get; set; }
            public string LanguageName { get; set; }
            public System.DateTime? Birthday { get; set; }
            public int CompanyId { get; set; }
            public int ConsultantCvId { get; set; }
            public bool IsUserSuperAdmin { get; set; }
            public bool IsUserLocalAdmin { get; set; }
            public DateTime TokenValidTo { get; set; }
            public bool Has2FAuthEnabled { get; set; }
            public int AdminLevel { get; set; }
        }

        public UserContainer User { get; set; }
    }
}
