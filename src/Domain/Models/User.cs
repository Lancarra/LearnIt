using System.Text.Json.Serialization;
using Domain.Models;

namespace Domain
{
    public class User
    {
        public User()
        {
        }
        public string Username { get; set; }  

        public int UserId { get; set; }

        public string Email { get; set; }

        public Guid? BlobId { get; set; }

        [JsonIgnore] public byte[] Hash { get; set; }

        [JsonIgnore] public byte[] Salt { get; set; }

        [JsonIgnore] public string? GoogleAuthKey { get; set; }

        [JsonIgnore] public bool IsGoogleAuthEnabled { get; set; }

        [JsonIgnore] public bool IsDeleted { get; set; }
        
        public virtual ICollection<CourseModule> CourseModules { get; set; }

    }
}