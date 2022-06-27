using Microsoft.AspNetCore.Mvc;
using TestWebApiTemplate;

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