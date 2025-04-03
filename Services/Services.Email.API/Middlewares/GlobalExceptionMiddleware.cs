using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Services.Email.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception | {Message}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Internal Server Error",
            Detail = _env.IsDevelopment() ? exception.Message : "Unexpected error occurred.",
            Instance = context.Request.Path
        };

        if (_env.IsDevelopment())
            problem.Extensions["stackTrace"] = exception.StackTrace;

        await context.Response.WriteAsJsonAsync(problem);
    }
}
