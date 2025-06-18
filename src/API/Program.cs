using System.Reflection;
using Domain;
using FluentValidation;
using Infrastructure;
using Infrastructure.Errors;
using Infrastructure.Database;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text.Json.Serialization;
using OpenApiSecurityScheme = NSwag.OpenApiSecurityScheme;



namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var  config = builder.Configuration;

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddInfrastructureServices(config);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(Assembly.Load("Application")); // <- Add this
            });
            builder.Services.AddValidatorsFromAssembly(Assembly.Load("Application"));
            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

            builder.Services.AddJwt();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
            });
            builder.Services.AddOpenApiDocument(document =>
            {
                document.Title = "LearnIt documentation";
                document.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Description = "Paste your JWT token into the input field.",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header
                });

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                //      new OperationSecurityScopeProcessor("Bearer"));
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:5178") 
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

            using var serviceScope = app.Services.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<LearnContext>();
            var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
            var passwordHasher = serviceScope.ServiceProvider.GetService<IPasswordHasher>();
            if (context != null)
            {
                context.Database.Migrate();

                var user = context.Users.SingleOrDefault(u =>
                    !u.IsDeleted && u.Email == "admin");
                var userPass = config.GetValue<string>("TestPassword");
                if (user == null && mediator != null && passwordHasher != null && userPass != null)
                {
                    var salt = Guid.NewGuid().ToByteArray();

                    {
                        var person = new User()
                        {
                            Email = "admin",
                            Hash = passwordHasher.Hash(userPass, salt),
                            Salt = salt
                        };

                        context.Users.Add(person);
                        context.SaveChanges();
                    }
                }
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            app.UseStatusCodePages();
            app.UseOpenApi();
            app.UseSwaggerUi(config => config.TransformToExternalPath = (internalUiRoute, request) =>
            {
                if (internalUiRoute.StartsWith("/") == true &&
                    internalUiRoute.StartsWith(request.PathBase) == false)
                {
                    return request.PathBase + internalUiRoute;
                }
                else
                {
                    return internalUiRoute;
                }
            });
            
            app.UseCors("AllowReactApp");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();



            app.Run();
        }
    }
}
