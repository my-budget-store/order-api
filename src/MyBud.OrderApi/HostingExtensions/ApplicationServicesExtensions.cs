using MyBud.OrdersApi.Interfaces;
using MyBud.OrdersApi.Repositories;

namespace MyBud.OrdersApi.HostingExtensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IOrderRepository, OrderRepository>()
                .AddHttpContextAccessor();
        }
    }
}
