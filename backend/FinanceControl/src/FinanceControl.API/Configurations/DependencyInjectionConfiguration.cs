using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using FinanceControl.Application.Services;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Repositories;
using FinanceControl.Persistence.Repositories;

namespace FinanceControl.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

            return services;
        }
    }
}
