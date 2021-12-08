using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Karma.Middleware
{
    public class StatisticsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public StatisticsMiddleware(RequestDelegate next, ILogger<StatisticsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var timer = new Stopwatch();
            timer.Start();
            context.Response.OnStarting(() => {
                timer.Stop();
                var elapsedMs = timer.ElapsedMilliseconds;
                _logger.LogInformation($"Request for {context.Request.Path} received ({context.Request.ContentLength ?? 0} bytes), response took {elapsedMs} ms");
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
