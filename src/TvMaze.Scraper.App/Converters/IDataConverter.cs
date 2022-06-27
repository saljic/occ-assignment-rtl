namespace TvMaze.Scraper.App.Converters;

public interface IDataConverter<in TDomain, out TDto>
{
    TDto Convert(TDomain domainModel);
}