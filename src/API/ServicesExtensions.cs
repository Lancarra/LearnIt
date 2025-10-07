using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace API
{
    public static class ServicesExtensions
    {
        public static void AddJwt(this IServiceCollection services)
        {
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey("egfwgtw4r32r2te5rty35241234f24ty3413e31qfwreg354yt34t13fw4ege5h34t2fq3vw4g234r23qrw4fgwrw"u8.ToArray());
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = "LearnIt";
            var audience = "LearnIt";

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            var token = context.HttpContext.Request.Headers["Authorization"];
                            if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token[0].Substring("Token ".Length).Trim();
                            }

                            return Task.CompletedTask;
                        }
                    };

                });
        }

        public static void AddSerilogLogging(this ILoggerFactory loggerFactory)
        {
            // Attach the sink to the logger configuration
            var log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                //just for local debug
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            Directory.CreateDirectory("SerilogLogs");

            var logToFile = new LoggerConfiguration()
                .MinimumLevel.Error()
                .Enrich.FromLogContext()
                .WriteTo.File(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}", path: "SerilogLogs/log.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger();

            loggerFactory.AddSerilog(log);
            loggerFactory.AddSerilog(logToFile);

            Log.Logger = log;
        }

        public static void AddLogForUnauthorizedError(this ILoggerFactory loggerFactory)
        {
            string path = "UnauthorizedErrorLogs";
            Directory.CreateDirectory(path);

            string hostName = Dns.GetHostName();

            // Get the IP from GetHostByName method of dns class.
            string IP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            Console.WriteLine("IP Address is : " + IP);
            int fCount = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
            string fileName = $"UnauthorizedLog{fCount}.txt";

            Directory.CreateDirectory("UnauthorizedErrorLogs");

            var pathString = System.IO.Path.Combine(path, fileName);
            if (!System.IO.File.Exists(pathString))
            {
                using (StreamWriter sw = File.AppendText(pathString))
                {
                    sw.WriteLineAsync("IP Address is : " + IP);
                    sw.WriteLineAsync("Date: " + DateTime.Now.ToString());
                    //sw.WriteLineAsync("Error : " + exception.StackTrace.Split('\n')[0]);


                }
            }

            var logToFile = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(
                    outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}",
                    path: pathString, rollingInterval: RollingInterval.Hour)
                .CreateLogger();


            loggerFactory.AddSerilog(logToFile);


        }
    }
}
