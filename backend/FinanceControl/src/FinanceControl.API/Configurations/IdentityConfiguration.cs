using FinanceControl.Domain.Entities;
using FinanceControl.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;

namespace FinanceControl.API.Configurations
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
