using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.App.Data.Dtos;

namespace TvMaze.Scraper.App.Converters;

public interface IShowConverter
{
    ShowDto Convert(Show show);
}