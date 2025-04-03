using FinanceControl.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.API.Configurations
{
    public static class DataBaseConfiguration
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}