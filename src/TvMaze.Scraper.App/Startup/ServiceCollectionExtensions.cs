using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestWebApiTemplate;

namespace TvMaze.Scraper.App.Startup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApp(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IShowConverter, ShowConverter>();
        
        serviceCollection.AddControllers();

        serviceCollection.AddEndpointsApiExplorer();

        serviceCollection.AddLogging(x => x.AddConsole());
        
        return serviceCollection;
    } 
}

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApp(this WebApplication webApplication)
    {
        webApplication.UseHttpsRedirection();
        webApplication.MapControllers();
        
        return webApplication;
    }
}