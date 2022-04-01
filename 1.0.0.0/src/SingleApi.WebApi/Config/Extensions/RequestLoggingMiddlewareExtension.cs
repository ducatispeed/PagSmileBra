using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SingleApi.WebApi.Config.Middlewares;

namespace SingleApi.WebApi.Config.Extensions
{
    public static class RequestLoggingMiddlewareExtension
    {
        public static IServiceCollection AddRequestLogging(this IServiceCollection services)
        {
            return services
                .AddHttpContextAccessor();
        }


        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}