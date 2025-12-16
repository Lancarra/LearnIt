using FluentValidation;

namespace Application.Users.GetAllStudentsByTeacherId;

public class GetAllStudentsByTeacherIdRequestValidator : AbstractValidator<GetAllStudentsByTeacherIdRequestDto>
{
    public GetAllStudentsByTeacherIdRequestValidator()
    {
        RuleFor(x => x.TeacherId).GreaterThan(0);
    }
}