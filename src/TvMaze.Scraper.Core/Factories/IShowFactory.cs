using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.Core.External.Data;

namespace TvMaze.Scraper.Core.Factories;

/// <summary>
///     The <see cref="IShowFactory" /> is used to create <see cref="Show" />'s
/// </summary>
public interface IShowFactory
{
    /// <summary>
    ///     Creates a <see cref="Show" /> from the provided <paramref name="showDto" /> and <paramref name="castDto" />
    /// </summary>
    Show Create(TvMazeShowDto showDto, IEnumerable<TvMazeCastDto> castDto);
}