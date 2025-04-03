using Services.Coupon.API.Middlewares;

namespace Services.Coupon.API.Extensions;
public static class GlobalExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
