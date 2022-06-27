using Microsoft.Extensions.DependencyInjection;
using Refit;
using TestWebApiTemplate;

namespace TvMaze.Scraper.Core.Startup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<DailyScrapingService>();

        serviceCollection.AddRefitClient<ITvMazeApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.tvmaze.com/"));

        serviceCollection.AddSingleton<IScrapingService, ScrapingService>();
        serviceCollection.AddTransient<IShowFactory, ShowFactory>();
        serviceCollection.AddSingleton<IShowRepository, InMemoryShowRepository>();
        return serviceCollection;
    } 
}