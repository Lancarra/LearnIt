using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class InfrastructureServiceExtension
{
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
              var connectionString = config.GetConnectionString("DefaultConnection");
              var databaseProvider = config.GetConnectionString("DatabaseProvider");

              services.AddScoped<IPasswordHasher, PasswordHasher>();
              services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
              services.TryAddScoped<ICurrentUserAccessor, CurrentUserAccessor.CurrentUserAccessor>();

              services.AddDbContext<LearnContext>(option =>
              {
                  
                  if (databaseProvider == "SqlServer")
                  {
                      option.UseSqlServer(connectionString);
                  }
                  else if(databaseProvider == "Postgresql")
                  {
                      option.UseNpgsql();
                  }
              });
        }
}