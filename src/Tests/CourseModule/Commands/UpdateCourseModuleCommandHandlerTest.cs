using Application.CourseModule.Update;
using Microsoft.EntityFrameworkCore;
using Tests.Common;

namespace Tests.CourseModule.Commands;

public class UpdateCourseModuleCommandHandlerTest : TestCommandBase
{
    [Fact]
    public async Task UpdateCourseModuleCommandHandlerSuccess()
    {
        var moduleId = Guid.NewGuid();
        var originalName = "Original Module";
        var updatedName = "Updated Module";
        var userId = 1;
        
        var courseModule = new Domain.Models.CourseModule
        {
            Id = moduleId,
            Name = originalName,
            UserId = userId
        };
        
        await Context.CourseModules.AddAsync(courseModule);
        await Context.SaveChangesAsync(CancellationToken.None);

        var handler = new UpdateModuleHandler(null!, Context);
        
        var result = await handler.Handle(new UpdateModuleRequestDto
        {
            Id = moduleId,
            Name = updatedName,
            UserId = userId
        }, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(moduleId, result.Id);
        Assert.Equal(updatedName, result.Name);
        Assert.Equal(userId, result.UserId);

        var updatedEntity = await Context.CourseModules.SingleOrDefaultAsync(cm => cm.Id == moduleId);
        Assert.NotNull(updatedEntity);
        Assert.Equal(updatedName, updatedEntity!.Name);
    }
}