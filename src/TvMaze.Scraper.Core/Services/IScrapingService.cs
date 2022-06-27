namespace TestWebApiTemplate;

public interface IScrapingService
{
    Task ScrapeTvMaze(int startPage);
}