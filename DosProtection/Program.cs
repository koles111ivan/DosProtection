using DosProtection.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<RateLimitingMiddleware>(new RateLimitingMiddleware.RateLimitOptions
{
    MaxRequests = 10, // 10 запросов для теста
    TimeWindow = TimeSpan.FromSeconds(30) // 30 секунд
});

app.MapControllers();
app.Run();