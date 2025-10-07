using Application.Users.Login;

namespace Application.Users.Create
{
    public class CreateUserResponseDto
    {
        public int UserId { get; set; }
        public Guid? BlobId { get; set; }
        public string UserName { get; set; }

        public LoginUserResponseDto  Response { get; set; }
    }
}
