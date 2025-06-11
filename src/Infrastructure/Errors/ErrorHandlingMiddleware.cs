using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Infrastructure.Errors
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, LearnContext dbContext)
        {
            try
            {
                if (context.Request.Path.Value == "/users/login")
                {
                    context.Request.EnableBuffering();
                }
                await _next(context);
                if (context.Request.Path.Value == "/users/login")
                {
                    await HandleAuthEventAsync(context, dbContext, true);
                }

            }
            catch (RestException re)
            {
                if (context.Request.Path.Value == "/users/login")
                {
                    await HandleAuthEventAsync(context, dbContext, false);
                }
                await HandleExceptionAsync(context, re, _logger);

            }
            catch (Exception ex)
            {
                if (context.Request.Path.Value == "/users/login")
                {
                    await HandleAuthEventAsync(context, dbContext, false);
                }
                await HandleExceptionAsync(context, ex, _logger);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context,Exception exception,ILogger<ErrorHandlingMiddleware> logger)
        {
            string result = null;
            switch (exception)
            {
                case RestException re:
                    context.Response.StatusCode = (int)re.Code;
                    result = JsonConvert.SerializeObject(new
                    {
                        errors = re.Errors
                    });
                    break;
                case ValidationException ve:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    List<string> errMsgLst = new List<string>();

                    foreach (ValidationFailure failure in ve.Errors)
                    {
                        errMsgLst.Add(failure.ErrorMessage);
                    }

                    result = JsonConvert.SerializeObject(new
                    {
                        errors = errMsgLst
                    });
                    break;
                case Exception e:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    logger.LogError(e, "Unhandled Exception");
                    result = JsonConvert.SerializeObject(new
                    {
                        errors = Constants.InternalServerError
                    });
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result ?? "{}");
        }

        private async Task HandleAuthEventAsync(HttpContext context, LearnContext dbContext, bool successful)
        {
            var (jsonDocument, requestUserJson) = await GetJsonElementFromContext(context, "user");
            string requestEmail = "";
            if (jsonDocument != null)
            {
                if (requestUserJson != null)
                {
                    if (requestUserJson.Value.TryGetProperty("email", out JsonElement emailElement))
                    {
                        requestEmail = emailElement.GetString();
                    }
                }
                jsonDocument.Dispose();
            }



            var logEvent = new LogEvent()
            {
                ClientIp = context.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                CalledEndpoint = context.Request.Path.Value,
                MethodType = context.Request.Method,
                EventType = successful ? Domain.Enums.LogEventType.AuthenticationSucceeded : Domain.Enums.LogEventType.AuthenticationFailed,
                Timestamp = DateTime.UtcNow,
                Email = requestEmail
            };
            logEvent.Description = successful ?
                $"[{logEvent.Timestamp}]: {logEvent.ClientIp} succesfully logged in as {logEvent.Email}" :
                $"[{logEvent.Timestamp}]: {logEvent.ClientIp} failed to log in as {logEvent.Email}";

            await dbContext.LogEvents.AddAsync(logEvent);
            await dbContext.SaveChangesAsync();
        }

        private async Task<(JsonDocument, JsonElement?)> GetJsonElementFromContext(HttpContext context, string propertyName)
        {
            try
            {
                var bodyStr = "";
                var request = context.Request;
                request.Body.Position = 0;

                using (StreamReader reader
                  = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }

                // Rewind, so the core is not lost when it looks at the body for the request
                request.Body.Position = 0;

                JsonDocument document = JsonDocument.Parse(bodyStr, new JsonDocumentOptions { AllowTrailingCommas = true });

                JsonElement root = document.RootElement;

                if (root.TryGetProperty(propertyName, out JsonElement property))
                {
                    return (document, property);
                }
                else
                {
                    document.Dispose();
                    return (null, null);
                }


            }
            catch (Exception ex)
            {
                return (null, null);
            }

        }
    }
}