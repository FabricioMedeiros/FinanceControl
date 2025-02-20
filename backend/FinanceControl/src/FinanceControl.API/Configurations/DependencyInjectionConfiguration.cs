using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using FinanceControl.Application.Services;

namespace FinanceControl.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
