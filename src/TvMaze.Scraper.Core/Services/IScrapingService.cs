namespace TvMaze.Scraper.Core.Services;

/// <summary>
///     The <see cref="IScrapingService" /> is used to scrape web apis
/// </summary>
public interface IScrapingService
{
    /// <summary>
    ///     Returns a <see cref="Task" /> which represents the asynchronous operation of scraping the tv maze api
    /// </summary>
    Task ScrapeTvMaze();
}