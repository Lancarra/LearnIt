using Application.CourseModule.Delete;
using Microsoft.EntityFrameworkCore;
using Tests.Common;

namespace Tests.CourseModule.Commands;

public class DeleteCourseModuleCommandHandlerTest : TestCommandBase
{
    [Fact]
    public async Task DeleteCourseModuleCommandHandlerSuccess()
    {
        var moduleId = Guid.NewGuid();
        var courseModule = new Domain.Models.CourseModule
        {
            Id = moduleId,
            Name = "Module is deleted",
            UserId = 1
        };
        await Context.CourseModules.AddAsync(courseModule);
        await Context.SaveChangesAsync(CancellationToken.None);
        
        var handler = new DeleteModuleHandler(null!, Context);
        
        await handler.Handle(new DeleteModuleRequestDto {Id = moduleId}, CancellationToken.None);

        Assert.Null(await Context.CourseModules.SingleOrDefaultAsync(cm => cm.Id == moduleId));
    }
}