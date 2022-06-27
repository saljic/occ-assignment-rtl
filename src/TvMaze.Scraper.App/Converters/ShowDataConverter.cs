using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.App.Data.Dtos;

namespace TvMaze.Scraper.App.Converters;

public sealed class ShowDataConverter : IDataConverter<Show, ShowDto>
{
    public ShowDto Convert(Show show)
    {
        var castDto = show.Cast.Select(x => new CastMemberDto(x.Id, x.Name, x.Birthday.ToString()));
        return new ShowDto(show.Id, show.Name, castDto);
    }
}

public sealed class DefaultDataConverter<TDomain, TData> : IDataConverter<TDomain, TData>
{
    public TData Convert(TDomain domainModel) =>
        throw new InvalidOperationException($"No data converter registered for {typeof(TDomain)}");
}