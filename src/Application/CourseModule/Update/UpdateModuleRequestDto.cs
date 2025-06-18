using MediatR;

namespace Application.CourseModule.Update;

public class UpdateModuleRequestDto : IRequest<UpdateModuleResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
}