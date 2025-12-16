using MediatR;

namespace Application.Users.UpdateRelation.UpdateTeacher;

public class UpdateTeacherRequestDto : IRequest<UpdateTeacherResponseDto>
{
    public int TeacherId { get; set; }
    public Operations Operation { get; set; }
    public List <int?> StudentsId { get; set; }
}