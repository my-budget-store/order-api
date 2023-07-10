using Microsoft.EntityFrameworkCore;
using MyBud.OrdersApi.Repositories;

namespace MyBud.OrdersApi.HostingExtensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            return services.AddDbContext<OrderContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("OrderContext")));
        }

        public static void UseConfiguredDatabase(this WebApplication app)
        {
            app.UseMiddleware<DatabaseProvider>();
        }

        private class DatabaseProvider
        {
            private readonly RequestDelegate _next;

            public DatabaseProvider(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext httpContext, OrderContext dbContext)
            {
                await dbContext.Database.MigrateAsync();
                await _next.Invoke(httpContext);
            }
        }
    }
}
