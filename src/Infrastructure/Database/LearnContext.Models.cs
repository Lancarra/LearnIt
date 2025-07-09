using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class LearnContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseModule>(m =>
        {
            m.HasKey(x => x.Id);
            m.Property(x => x.Name).IsRequired();
            m.HasMany(x => x.Folders)
                .WithOne(x => x.CourseModule)
                .HasForeignKey(x => x.CourseModuleId);
        });
        
        modelBuilder.Entity<Folder>(m =>
        {
            m.HasKey(x => x.Id);
            m.Property(x => x.Name).IsRequired();
            m.HasMany(x => x.Dictionaries)
                .WithOne(x => x.ParentFolder)
                .HasForeignKey(x => x.ParentFolderId);
        });

        modelBuilder.Entity<LearnWordDictionary>(m =>
        {
            m.HasKey(x => x.Id);
            m.Property(x => x.Name).IsRequired();
            m.HasMany(x => x.Definitions)
                .WithOne(x => x.Dictionary)
                .HasForeignKey(x => x.DictionaryId);
        });

        modelBuilder.Entity<Definition>(m =>
        {
            m.HasKey(x => x.Id);
            m.Property(x => x.Word).IsRequired();
            m.Property(x => x.Meaning).IsRequired();
            m.Property(x => x.BlobId);
            m.HasOne(x => x.Dictionary)
                .WithMany(x => x.Definitions);
        });

        modelBuilder.Entity<Role>(r =>
        {
            r.HasKey(k => k.RoleId);
            r.Property(p => p.RoleId).ValueGeneratedOnAdd();
            r.Property(k => k.RoleName).IsRequired();
        });
        
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
        
        modelBuilder.Entity<Achievement>(r =>
            {
                r.HasKey(k => k.Id);
                r.Property(p => p.Id).ValueGeneratedOnAdd();
                r.HasMany(u => u.Users);
            });
            
    }
}