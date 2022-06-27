using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.App.Converters;
using TvMaze.Scraper.App.Data.Dtos;

namespace TvMaze.Scraper.App.Startup;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApp(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient(typeof(IDataConverter<,>), typeof(DefaultDataConverter<,>));
        serviceCollection.AddTransient<IDataConverter<Show, ShowDto>, ShowDataConverter>();

        serviceCollection.AddControllers();

        serviceCollection.AddEndpointsApiExplorer();

        serviceCollection.AddLogging(x => x.AddConsole());

        return serviceCollection;
    }
}