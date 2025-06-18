using FluentValidation;

namespace Application.Users.Delete;

public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequestDto>
{
    public DeleteUserRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}