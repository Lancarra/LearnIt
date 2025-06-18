namespace Infrastructure.Security
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string email, int expiresAfterMinutes);
    }
}
