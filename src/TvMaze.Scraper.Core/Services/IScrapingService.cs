namespace TvMaze.Scraper.Core.Services;

public interface IScrapingService
{
    Task ScrapeTvMaze(int startPage);
}