using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;

namespace TvMaze.Scraper.App.Startup;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApp(this WebApplication webApplication)
    {
        webApplication.UseHttpsRedirection();
        webApplication.MapControllers();

        return webApplication;
    }
}