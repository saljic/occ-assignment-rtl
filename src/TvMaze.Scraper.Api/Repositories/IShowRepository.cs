using TvMaze.Scraper.Api.DomainModels;

namespace TvMaze.Scraper.Api.Repositories;

/// <summary>
///     The <see cref="IShowRepository" /> is used to access show data
/// </summary>
public interface IShowRepository
{
    /// <summary>
    ///     Returns a <see cref="Task" /> which represents the asynchronous operation of adding or updating the provided
    ///     <paramref name="show" />
    /// </summary>
    Task AddOrUpdateAsync(Show show);

    /// <summary>
    ///     Returns a <see cref="Task" /> which represents the asynchronous operation of getting all the <see cref="Show" />'s
    ///     that are available
    /// </summary>
    Task<IEnumerable<Show>> GetAllAsync();
}