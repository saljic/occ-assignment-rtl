namespace TvMaze.Scraper.Core.Services;

/// <summary>
///     The <see cref="IScrapingService" /> is used to scrape web apis
/// </summary>
public interface IScrapingService
{
    /// <summary>
    ///     Returns a <see cref="Task" /> which represents the asynchronous operation of scraping the tv maze api from the
    ///     provided <paramref name="startPage" /> til the last page
    /// </summary>
    Task ScrapeTvMaze(int startPage);
}