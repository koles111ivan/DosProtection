using DosProtection.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<RateLimitingMiddleware>(new RateLimitingMiddleware.RateLimitOptions
{
    MaxRequests = 10, 
    TimeWindow = TimeSpan.FromSeconds(30)
});

app.MapControllers();
app.Run();