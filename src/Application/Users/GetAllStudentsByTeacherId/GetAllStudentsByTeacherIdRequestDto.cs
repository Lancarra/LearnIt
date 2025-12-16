using MediatR;

namespace Application.Users.GetAllStudentsByTeacherId;

public class GetAllStudentsByTeacherIdRequestDto : IRequest<GetAllStudentsByTeacherIdResponseDto>
{
    public int TeacherId { get; set; }
}