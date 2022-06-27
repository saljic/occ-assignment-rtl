using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.Core.External.Data;

namespace TvMaze.Scraper.Core.Factories;

public interface IShowFactory
{
    Show Create(TvMazeShowDto showDto, IEnumerable<TvMazeCastDto> castDto);
}