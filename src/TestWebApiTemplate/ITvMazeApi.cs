using Refit;

namespace TestWebApiTemplate;

public interface ITvMazeApi
{
    [Get("/shows?page={pageNumber}")]
    Task<IReadOnlyCollection<TvMazeShowDto>> GetShows(int pageNumber);

    [Get("/shows/{showId}/cast")]
    Task<IReadOnlyCollection<TvMazeCastDto>> GetCast(int showId);
}