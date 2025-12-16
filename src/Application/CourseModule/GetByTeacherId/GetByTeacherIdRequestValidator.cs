using FluentValidation;

namespace Application.CourseModule.GetByTeacherId;

public class GetByTeacherIdRequestValidator : AbstractValidator<GetByTeacherIdRequestDto>
{
    public GetByTeacherIdRequestValidator()
    {
        RuleFor(x => x.TeacherId).NotEmpty().GreaterThan(0);
    }
}