using System.ComponentModel;

namespace Application.Users.Login.ServiceLoginRequests
{
    public class LearnItRequest
    {
        public class UserRequest
        {
            public string Email { get; set; }

            public string Password { get; set; }

            public string GoogleAuthKey { get; set; }

            [DefaultValue(false)]
            public bool FromAnotherService { get; set; }
        }

        public UserRequest User { get; set; }
    }
}
