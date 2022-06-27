using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.Core.External.Data;

namespace TvMaze.Scraper.Core.Factories;

public sealed class ShowFactory : IShowFactory
{
    public Show Create(TvMazeShowDto showDto, IEnumerable<TvMazeCastDto> castDto)
    {
        var cast = castDto
            .Select(x => new CastMember(x.Person.Id, x.Person.Name,
                x.Person.Birthday == null ? null : DateOnly.Parse(x.Person.Birthday)))
            .OrderByDescending(x => x.Birthday)
            .ToArray();
        return new Show(showDto.Id, showDto.Name, cast);
    }
}