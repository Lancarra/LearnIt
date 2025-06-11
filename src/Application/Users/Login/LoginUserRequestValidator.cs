using FluentValidation;

namespace Application.Users.Login
{
    class LoginUserRequestValidator : AbstractValidator<LoginUserRequestDto>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
