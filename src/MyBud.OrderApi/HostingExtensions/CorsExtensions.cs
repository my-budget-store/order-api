namespace MyBud.OrdersApi.HostingExtensions
{
    public static class CorsExtensions
    {
        static string GatewayPolicy = "GatewayPolicy";

        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddCors(options =>
                {
                    options.AddPolicy(name: GatewayPolicy,
                        builder =>
                        {
                            builder.WithOrigins(config.GetSection("AllowedOrigins").Get<string[]>())
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
                });

            return services;
        }

        public static void UseConfiguredCors(this WebApplication app)
        {
            app.UseCors(GatewayPolicy);
        }
    }
}
