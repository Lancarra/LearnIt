namespace Infrastructure.CurrentUserAccessor
{
    public interface ICurrentUserAccessor
    {
        string? GetCurrentEmail();
        string? GetCurrentRoles();
    }
}
