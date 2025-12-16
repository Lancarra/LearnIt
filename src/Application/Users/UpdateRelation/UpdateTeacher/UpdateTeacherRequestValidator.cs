using FluentValidation;

namespace Application.Users.UpdateRelation.UpdateTeacher;

public class UpdateTeacherRequestValidator : AbstractValidator<UpdateTeacherRequestDto>
{
    public UpdateTeacherRequestValidator()
    {
        RuleFor(x => x.TeacherId).NotNull().NotEmpty();
        RuleFor(x => x.StudentsId).NotNull().NotEmpty();
        RuleFor(x => x.Operation).NotNull().NotEmpty();
    }
}