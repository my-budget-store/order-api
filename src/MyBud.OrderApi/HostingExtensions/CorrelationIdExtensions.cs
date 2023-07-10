namespace MyBud.OrdersApi.HostingExtensions
{
    public static class CorrelationIdExtensions
    {
        private const string CorrelationIdHeaderKey = "X-Correlation-ID";

        public static void UseConfiguredCorrelationContext(this WebApplication app)
        {
            app.Use(async (ctx, next) =>
            {
                if (!ctx.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out var correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();
                    ctx.Request.Headers.Add(CorrelationIdHeaderKey, correlationId);
                }

                ctx.Response.Headers.Add(CorrelationIdHeaderKey, correlationId);

                await next.Invoke(ctx);
            });
        }
    }
}