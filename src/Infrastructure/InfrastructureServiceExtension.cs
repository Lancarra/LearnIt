using Azure.Storage;
using Azure.Storage.Blobs;
using Infrastructure.BlobStorage.Service;
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
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
              var connectionString = configuration.GetConnectionString("DefaultConnection");
              var databaseProvider = configuration.GetConnectionString("DatabaseProvider");

              services.AddScoped<IPasswordHasher, PasswordHasher>();
              services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
              services.TryAddScoped<ICurrentUserAccessor, CurrentUserAccessor.CurrentUserAccessor>();


              var blobSettings = configuration.GetSection("BlobSettings");
              var credential = new StorageSharedKeyCredential(blobSettings["AccountName"], blobSettings["AccountKey"]);
              var uri = blobSettings["Uri"];
              services.AddSingleton<IBlobService, BlobService>();
              services.AddSingleton(b => new BlobServiceClient(new Uri(uri), credential));
             
              services.AddDbContext<LearnContext>(option =>
              {
                  
                  if (databaseProvider == "SqlServer")
                  {
                      option.UseSqlServer(connectionString);
                  }
                  else if(databaseProvider == "Postgresql")
                  {
                      option.UseNpgsql(connectionString);
                  }
              });
        }
}