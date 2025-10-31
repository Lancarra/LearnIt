using Domain;
using Domain.Models;
using Domain.Models.Quiz;
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
            
        modelBuilder.Entity<TestCard>(tc =>
        {
            tc.HasKey(k => k.Id);
            tc.HasOne(q => q.Dictionary)
                .WithMany(d => d.TestCards)
                .HasForeignKey(k =>  k.DictionaryId)
                .OnDelete(DeleteBehavior.NoAction);
            tc.HasMany(u => u.TestUnits)
                .WithOne(u => u.TestCard)
                .HasForeignKey(u => u.TestCardId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<TestUnit>()
            .HasOne(tc => tc.TestCard)
            .WithMany(r => r.TestUnits)
            .HasForeignKey(ur => ur.TestCardId).OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<TestCardAnswer>(tc =>
        {
            tc.HasKey(k => k.Id);
            tc.HasMany(u => u.TestUnitAnswers)
                .WithOne(u => u.TestCardAnswer)
                .HasForeignKey(u => u.TestCardAnswerId)
                .OnDelete(DeleteBehavior.Cascade);
            tc.HasOne<TestCard>(tca => tca.TestCard);
        });
        
        modelBuilder.Entity<TestUnitAnswers>(tua =>
        {
            tua.HasKey(k => k.Id);
            tua.HasOne(u => u.TestCardAnswer)
                .WithMany(u => u.TestUnitAnswers)
                .HasForeignKey(u => u.TestCardAnswerId)
                .OnDelete(DeleteBehavior.Cascade);
            tua.HasOne<TestUnit>(tca => tca.TestUnit);
        });
        
        modelBuilder.Entity<StaticImages>(si =>
        {
            si.HasKey(k => k.FileBlobId);
        });
    }
}