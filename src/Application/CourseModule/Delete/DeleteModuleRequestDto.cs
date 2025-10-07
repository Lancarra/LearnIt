using MediatR;

namespace Application.CourseModule.Delete;

public class DeleteModuleRequestDto : IRequest<DeleteModuleResponseDto>
{
    public Guid Id { get; set; }
}

