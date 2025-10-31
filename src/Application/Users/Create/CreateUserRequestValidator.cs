using FluentValidation;

namespace Application.Users.Create;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
        RuleFor(x => x.RoleName).NotNull().NotEmpty();
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
    }
}