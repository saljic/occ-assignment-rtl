using Refit;
using TvMaze.Scraper.Core.External.Data;

namespace TvMaze.Scraper.Core.External;

public interface ITvMazeApi
{
    [Get("/shows?page={pageNumber}")]
    Task<IReadOnlyCollection<TvMazeShowDto>> GetShows(int pageNumber);

    [Get("/shows/{showId}/cast")]
    Task<IReadOnlyCollection<TvMazeCastDto>> GetCast(int showId);
}