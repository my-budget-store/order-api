using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace MyBud.OrdersApi.HostingExtensions
{
    public static class MvcExtensions
    {
        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    options.JsonSerializerOptions.Converters.Add(enumConverter);
                });

            return services;
        }
    }
}
