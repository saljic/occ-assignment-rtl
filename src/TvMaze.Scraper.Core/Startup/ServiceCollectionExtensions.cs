using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using TvMaze.Scraper.Api.Repositories;
using TvMaze.Scraper.Core.Configuration;
using TvMaze.Scraper.Core.External;
using TvMaze.Scraper.Core.Factories;
using TvMaze.Scraper.Core.Repositories;
using TvMaze.Scraper.Core.Services;

namespace TvMaze.Scraper.Core.Startup;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService(x => new DailyScrapingService(x.GetRequiredService<IScrapingService>(), TaskPoolScheduler.Default, x.GetRequiredService<ILogger<DailyScrapingService>>()));

        serviceCollection.AddRefitClient<ITvMazeApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.tvmaze.com/"));

        serviceCollection.AddSingleton<IScrapingService, ScrapingService>();
        serviceCollection.AddTransient<IShowFactory, ShowFactory>();
        serviceCollection.AddSingleton(new ScrapingServiceConfiguration(TimeSpan.FromSeconds(11)));
        serviceCollection.AddSingleton<IShowRepository, InMemoryShowRepository>();
        return serviceCollection;
    }
}