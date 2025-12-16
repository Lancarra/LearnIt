using MediatR;

namespace Application.CourseModule.GetByTeacherId;

public class GetByTeacherIdRequestDto : IRequest<GetByTeacherIdResponseDto>
{
    public int TeacherId { get; set; }
}