using FluentValidation;

namespace Application.Users.GetById;

public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequestDto>
{
    public GetUserByIdRequestValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}