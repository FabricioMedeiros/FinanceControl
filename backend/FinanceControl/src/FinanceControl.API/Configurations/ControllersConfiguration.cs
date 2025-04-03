using FinanceControl.API.Filters;

namespace FinanceControl.API.Configurations
{
    public static class ControllersConfiguration
    {
        public static void AddControllersConfiguration(this IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    });
        }
    }
}
