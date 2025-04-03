using Services.Order.API.Middlewares;

namespace Services.Order.API.Extensions;
public static class GlobalExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}   
