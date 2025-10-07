using Domain;
using Domain.Models;
using Domain.Models.Quiz;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class LearnContext
{
    public virtual DbSet<CourseModule> CourseModules { get; set; }
    public virtual DbSet<Folder> Folders { get; set; }
    public virtual DbSet<LearnWordDictionary> LearnWordDictionaries { get; set; }
    public virtual DbSet<Definition> Definitions { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<TestCard> TestCards { get; set; }
    public virtual DbSet<TestUnit> TestUnits { get; set; }
    public virtual DbSet<TestCardAnswer> TestCardAnswer { get; set; }
    public virtual DbSet<TestUnitAnswers> TestUnitAnswers { get; set; }
    public virtual DbSet<LogEvent> LogEvents { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    
    public virtual DbSet<Achievement> Achievements { get; set; }
    public virtual DbSet<UserRole> UserRole { get; set; }
    
}