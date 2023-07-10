using Microsoft.AspNetCore.Mvc;

namespace MyBud.OrdersApi.HostingExtensions
{
    public static class ApiVersioningAndExplorerExtensions
    {
        public static IServiceCollection ConfigureApiVersioningAndExplorer(this IServiceCollection services)
        {
            return services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            })
            .AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            })
            .AddEndpointsApiExplorer();
        }
    }
}
