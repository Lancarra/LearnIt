using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Models.Quiz;

namespace Domain
{
    public class User
    {
        public User()
        {
            TeachersId = new List<int?>();
            StudentsId = new List<int?>();
            OwnedCourseModules = new List<CourseModule>();
            StudentCourseModules = new List<CourseModule>();
        }
        public string Username { get; set; }  

        public int UserId { get; set; }

        public string Email { get; set; }

        public Guid? BlobId { get; set; }
        public List <int?> TeachersId { get; set; }
        public List <int?> StudentsId { get; set; }
        
        [JsonIgnore] public byte[] Hash { get; set; }

        [JsonIgnore] public byte[] Salt { get; set; }

        [JsonIgnore] public string? GoogleAuthKey { get; set; }

        [JsonIgnore] public bool IsGoogleAuthEnabled { get; set; }

        [JsonIgnore] public bool IsDeleted { get; set; }
        
        public virtual ICollection<CourseModule> OwnedCourseModules { get; set; }
        
        public virtual ICollection<CourseModule> StudentCourseModules { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        public virtual Achievement? Achievement { get; set; }
        public int? AchievementId { get; set; }
        public int Rating { get; set; }
        
        public virtual ICollection<TestCardAnswer> TestCardAnswer { get; set; }
        
        
    }
}