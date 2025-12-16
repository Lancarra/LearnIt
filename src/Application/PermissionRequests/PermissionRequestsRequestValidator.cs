using FluentValidation;

namespace Application.PermissionRequests;

public class PermissionRequestsRequestValidator : AbstractValidator<PermissionRequestsRequestDto>
{
    public PermissionRequestsRequestValidator()
    {
        RuleFor(x => x.RequestUserId).GreaterThan(0);
        RuleFor(x => x.RoleName).NotEmpty().NotNull();
    }
}