using Services.ShoppingCart.API.Middlewares;

namespace Services.ShoppingCart.API.Extensions;
public static class GlobalExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}   
