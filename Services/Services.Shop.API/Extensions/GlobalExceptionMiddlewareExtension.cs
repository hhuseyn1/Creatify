using Services.Shop.API.Middlewares;

namespace Services.Shop.API.Extensions;
public static class GlobalExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}   
