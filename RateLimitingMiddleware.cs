using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DosProtection
{
    public class RateLimitingMiddleware
    {
        public class Options
        {
            public int MaxRequests { get; set; }
            public TimeSpan TimeWindow { get; set; }
        }
        // Конструктор
        public RateLimitingMiddleware(RequestDelegate next, int maxRequests, TimeSpan timeWindow)
        {
            _next = next;
            _maxRequests = maxRequests;
            _timeWindow = timeWindow;
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

            // Сброс счетчика если прошел тайм-аут
            if (currentTime - clientInfo.LastResetTime > _timeWindow)
            {
                clientInfo.Reset(_timeWindow);
            }

            // Проверка лимита
            if (clientInfo.RequestCount >= _maxRequests)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync($"Rate limit exceeded. Try again in {_timeWindow.TotalSeconds} seconds.");
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