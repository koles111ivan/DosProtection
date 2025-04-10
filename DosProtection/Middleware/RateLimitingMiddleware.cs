using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DosProtection.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimitOptions _options;
        private static readonly ConcurrentDictionary<string, ClientRequestInfo> _clientRequests = new();

        public class RateLimitOptions
        {
            public int MaxRequests { get; set; }
            public TimeSpan TimeWindow { get; set; }
        }

        public RateLimitingMiddleware(RequestDelegate next, RateLimitOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            if (!_clientRequests.TryGetValue(clientIp, out var clientInfo))
            {
                clientInfo = new ClientRequestInfo();
                _clientRequests[clientIp] = clientInfo;
            }

            var currentTime = DateTime.UtcNow;

            if (currentTime - clientInfo.LastResetTime > _options.TimeWindow)
            {
                clientInfo.Reset(_options.TimeWindow);
            }

            if (clientInfo.RequestCount >= _options.MaxRequests)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync($"Rate limit exceeded. Try again in {_options.TimeWindow.TotalSeconds} seconds.");
                return;
            }

            clientInfo.IncrementRequestCount();
            await _next(context);
        }

        private class ClientRequestInfo
        {
            public int RequestCount { get; private set; }
            public DateTime LastResetTime { get; private set; } = DateTime.UtcNow;

            public void IncrementRequestCount() => RequestCount++;

            public void Reset(TimeSpan timeWindow)
            {
                RequestCount = 1;
                LastResetTime = DateTime.UtcNow;
            }
        }
    }
}