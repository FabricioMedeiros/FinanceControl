using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using FinanceControl.Application.Services;
using FinanceControl.Application.Validators;
using FinanceControl.Domain.Interfaces;
using FinanceControl.Infrastructure.Mappings;
using FinanceControl.Infrastructure.Repositories;

namespace FinanceControl.API.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            //Notificator
            services.AddScoped<INotificator, Notificator>();

            //Jwt
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            //Http Context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MappingProfile));

            //Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

            //Validators
            services.AddScoped<CategoryCreateValidator>();
            services.AddScoped<CategoryUpdateValidator>();
            services.AddScoped<PaymentMethodCreateValidator>();
            services.AddScoped<PaymentMethodUpdateValidator>();
            services.AddScoped<TransactionCreateValidator>();
            services.AddScoped<TransactionUpdateValidator>();

            return services;
        }
    }
}
