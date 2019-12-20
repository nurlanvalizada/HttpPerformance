using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HttpPerformance.Middlewares
{
    public class ResponseTimeMiddleware
    {
        private const string RESPONSE_HEADER_RESPONSE_TIME = "X-Response-Time-Ms";

        // Handle to the next Middleware in the pipeline  
        private readonly RequestDelegate _next;
        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            // Start the Timer using Stopwatch  
            var watch = new Stopwatch();
            watch.Start();

            context.Response.OnStarting(() => {
                // Stop the timer information and calculate the time   
                watch.Stop();

                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;

                // Add the Response time information in the Response headers.   
                context.Response.Headers[RESPONSE_HEADER_RESPONSE_TIME] = responseTimeForCompleteRequest.ToString();
                return Task.CompletedTask;
            });

            // Call the next delegate/middleware in the pipeline   
            return this._next(context);
        }
    }
}
