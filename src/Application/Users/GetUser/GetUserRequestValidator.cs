using FluentValidation;

namespace Application.Users.GetById;

public class GetUserRequestValidator : AbstractValidator<GetUserRequestDto>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}