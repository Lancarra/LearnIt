using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Tests.Common;

public class LearnItContextFactory
{
    public static LearnContext Create()
    {
        var option = new DbContextOptionsBuilder<LearnContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;
        
        var context = new LearnContext(option);
        context.Database.EnsureCreated();
        context.CourseModules.AddRange(
        new Domain.Models.CourseModule
        {
            Name = "First",
            UserId = 1,
        },
        new Domain.Models.CourseModule
        {
            Name = "Second",
            UserId = 2,
        });
        
        return context;
    }

    public static void Destroy(LearnContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}