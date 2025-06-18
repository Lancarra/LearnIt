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
            m.Property(x => x.BlobURL);
            m.HasOne(x => x.Dictionary)
                .WithMany(x => x.Definitions);
        });
    }
}