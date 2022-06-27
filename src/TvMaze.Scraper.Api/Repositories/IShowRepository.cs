using TvMaze.Scraper.Api.DomainModels;

namespace TvMaze.Scraper.Api.Repositories;

public interface IShowRepository
{
    Task AddOrUpdateAsync(Show show);
    Task<IEnumerable<Show>> GetAllAsync();
}