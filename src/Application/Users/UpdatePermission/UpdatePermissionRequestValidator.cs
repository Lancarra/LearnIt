using FluentValidation;

namespace Application.Users.UpdatePermission;

public class UpdatePermissionRequestValidator : AbstractValidator<UpdatePermissionRequestDto>
{
    public UpdatePermissionRequestValidator()
    {
        RuleFor(x => x.RoleId).NotNull().NotEmpty().GreaterThan(0);
        RuleFor(x => x.UserId).NotNull().NotEmpty().GreaterThan(0);
    }
}