using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Creatify.Shared;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception caught by GlobalExceptionFilter");

        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred!",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = context.Exception.Message  
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
        context.ExceptionHandled = true;
    }
}
