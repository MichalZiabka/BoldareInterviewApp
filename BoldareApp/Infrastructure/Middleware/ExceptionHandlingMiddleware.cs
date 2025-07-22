using BoldareApp.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BoldareApp.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var problemDetails = HandleException(exception, context);

                context.Response.StatusCode = problemDetails.Status ?? 500;
                context.Response.ContentType = "application/problem+json";

                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
        }

        private ProblemDetails HandleException(Exception exception, HttpContext context)
        {
            return exception switch
            {
                HttpRequestException or ExternalApiException => CreateProblemDetailsAndLog(
                    type: "https://httpstatuses.com/502",
                    title: "External API error",
                    detail: exception.Message,
                    status: 502,
                    instance: context.Request.Path,
                    level: LogLevel.Warning,
                    exception: exception
                ),
                ArgumentException => CreateProblemDetailsAndLog(
                    type: "https://httpstatuses.com/400",
                    title: "Bad request",
                    detail: exception.Message,
                    status: 400,
                    instance: context.Request.Path,
                    level: LogLevel.Information,
                    exception: exception
                ),
                _ => CreateProblemDetailsAndLog(
                    type: "https://httpstatuses.com/500",
                    title: "Internal server error",
                    detail: exception.Message,
                    status: 500,
                    instance: context.Request.Path,
                    level: LogLevel.Error,
                    exception: exception
                ),
            };
        }

        private ProblemDetails CreateProblemDetailsAndLog(string type, string title, string detail, int status, string instance, LogLevel level, Exception exception)
        {
            _logger.Log(level, exception, "{Title}: {Detail}", title, detail);

            return new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{status}",
                Title = title,
                Detail = detail,
                Status = status,
                Instance = instance
            };
        }
    }
}