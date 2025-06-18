using FluentValidation;

namespace Application.Users.Read
{
    public class ReadUserRequestValidator : AbstractValidator<ReadUserRequestDTO>
    {
        public ReadUserRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
