using Refit;
using TestWebApiTemplate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<DailyScrapingService>();

builder.Services.AddRefitClient<ITvMazeApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.tvmaze.com/"));

builder.Services.AddSingleton<IScrapingService, ScrapingService>();
builder.Services.AddTransient<IShowFactory, ShowFactory>();
builder.Services.AddTransient<IShowConverter, ShowConverter>();
builder.Services.AddSingleton<IShowRepository, InMemoryShowRepository>();

builder.Services.AddLogging(x => x.AddConsole());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();