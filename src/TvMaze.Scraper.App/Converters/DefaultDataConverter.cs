namespace TvMaze.Scraper.App.Converters;

public sealed class DefaultDataConverter<TDomain, TData> : IDataConverter<TDomain, TData>
{
    public TData Convert(TDomain domainModel)
    {
        throw new InvalidOperationException($"No data converter registered for {typeof(TDomain)}");
    }
}