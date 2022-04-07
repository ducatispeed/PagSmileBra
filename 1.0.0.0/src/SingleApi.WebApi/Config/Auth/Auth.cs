using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SfpSharedLib.Api.Extensions;

namespace SingleApi.WebApi.Config.Auth
{
    public static class Auth
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiAuthentication(configuration);

            return services;
        }

        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app
                .UseAuthorization()
                .UseAuthentication();

            return app;
        }
    }
}