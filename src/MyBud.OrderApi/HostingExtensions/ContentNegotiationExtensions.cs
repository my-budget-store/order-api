namespace MyBud.OrdersApi.HostingExtensions
{
    public static class ContentNegotiationExtensions
    {
        public static void UseConfiguredContentNegotiation(this WebApplication app)
        {
            app.Use(async (ctx, next) =>
            {
                string acceptHeader = ctx.Request.Headers["Accept"];

                if (acceptHeader.Contains("application/json"))
                {
                    ctx.Response.Headers["Content-Type"] = "application/json";
                }
                else if (acceptHeader.Contains("application/xml"))
                {
                    ctx.Response.Headers["Content-Type"] = "application/xml";
                }

                await next.Invoke(ctx);
            });
        }
    }
}
