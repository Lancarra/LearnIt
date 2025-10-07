using Application.CourseModule.Create;
using Microsoft.EntityFrameworkCore;
using Tests.Common;

namespace Tests.CourseModule.Commands;

public class CreateCourseModuleCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateCourseModuleCommandHandlerSuccess()
    {
        //Arrange
        var handler = new CreateModuleHandler(Context);
        var name = "First Test";
        var userId = 1;
        
        //Act
        var courseResponse = await handler.Handle(
            new CreateModuleRequestDto
            {
                Name = name,
                UserId = userId
            }, CancellationToken.None);
        
        //Assert
        Assert.NotNull(await Context.CourseModules.SingleOrDefaultAsync
            (cr => cr.Name == name 
                            && cr.UserId == userId
                            && cr.Id == courseResponse.Id));
    }
}