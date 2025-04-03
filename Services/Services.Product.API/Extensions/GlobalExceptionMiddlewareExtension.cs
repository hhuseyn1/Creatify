using Services.Product.API.Middlewares;

namespace Services.Product.API.Extensions;
public static class GlobalExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}   
