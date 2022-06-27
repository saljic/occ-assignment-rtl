namespace TvMaze.Scraper.App.Converters;

/// <summary>
///     The <see cref="IDataConverter{TDomain,TDto}" /> is used to convert domain models to dtos
/// </summary>
public interface IDataConverter<in TDomain, out TDto>
{
    /// <summary>
    ///     Converts the provided <typeparamref name="TDomain" /> to <typeparamref name="TDto" />
    /// </summary>
    TDto Convert(TDomain domainModel);
}