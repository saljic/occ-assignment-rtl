using Microsoft.AspNetCore.Mvc;
using TvMaze.Scraper.Api.Repositories;
using TvMaze.Scraper.App.Converters;
using TvMaze.Scraper.App.Data.Dtos;

namespace TvMaze.Scraper.App.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ShowsController : ControllerBase
{
    private readonly IShowConverter _showConverter;
    private readonly IShowRepository _showRepository;

    public ShowsController(IShowRepository showRepository, IShowConverter showConverter)
    {
        _showRepository = showRepository;
        _showConverter = showConverter;
    }

    [HttpGet(Name = "GetShows")]
    public async Task<IEnumerable<ShowDto>> Get()
    {
        var shows = await _showRepository.GetAllAsync();
        return shows.Select(_showConverter.Convert);
    }
}