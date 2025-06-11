using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class LearnContext
{
    public virtual DbSet<CourseModule> CourseModules { get; set; }
    public virtual DbSet<Folder> Folders { get; set; }
    public virtual DbSet<LearnWordDictionary> LearnWordDictionaries { get; set; }
    public virtual DbSet<Definition> Definitions { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<LogEvent> LogEvents { get; set; }
}