using Microsoft.Extensions.Caching.Distributed;

namespace MyBud.OrdersApi.HostingExtensions
{
    public static class CachingExtensions
    {
        public static void UseConfiguredCachingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<CachingMiddleware>();
        }

        private class CachingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IDistributedCache _cache;

            public CachingMiddleware(RequestDelegate next, IDistributedCache cache)
            {
                _next = next;
                _cache = cache;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                // Generate a unique cache key based on the request URL
                string cacheKey = context.Request.Path.ToString();

                // Try to get the response from the cache
                byte[] cachedResponse = await _cache.GetAsync(cacheKey);

                if (cachedResponse != null)
                {
                    // Serve the response from cache
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.Body.WriteAsync(cachedResponse);
                }
                else
                {
                    // Create a custom response stream to capture the response
                    using (var customResponseBody = new MemoryStream())
                    {
                        // Replace the original response stream with the custom response stream
                        var originalResponseBody = context.Response.Body;
                        context.Response.Body = customResponseBody;

                        // Call the next middleware in the pipeline
                        await _next(context);

                        // Check if the response is cacheable (200 OK status)
                        if (context.Response.StatusCode == StatusCodes.Status200OK)
                        {
                            // Cache the response
                            cachedResponse = customResponseBody.ToArray();
                            var cacheOptions = new DistributedCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Set cache expiration time
                            };
                            await _cache.SetAsync(cacheKey, cachedResponse, cacheOptions);
                        }

                        // Copy the custom response stream to the original response stream
                        customResponseBody.Seek(0, SeekOrigin.Begin);
                        await customResponseBody.CopyToAsync(originalResponseBody);
                        context.Response.Body = originalResponseBody;
                    }
                }
            }
        }
    }
}
