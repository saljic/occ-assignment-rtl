using Microsoft.AspNetCore.Mvc;
using TvMaze.Scraper.Api.Repositories;
using TvMaze.Scraper.App.Converters;
using TvMaze.Scraper.App.Data.Dtos;
using TvMaze.Scraper.App.Data.Requests;

namespace TvMaze.Scraper.App.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
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
    public async Task<IActionResult> Get([FromQuery] PaginationQuery paginationQuery)
    {
        if (paginationQuery.PageIndex < 0 || paginationQuery.ItemCount <= 0)
        {
            return BadRequest(ArraySegment<ShowDto>.Empty);
        }
        
        var shows = await _showRepository.GetAllAsync();
        var showsList = shows.ToArray();

        var lastItemIndex = (paginationQuery.PageIndex + 1) * paginationQuery.ItemCount;
        var firstItemIndex = paginationQuery.PageIndex * paginationQuery.ItemCount;

        if (firstItemIndex > showsList.Length)
        {
            return Ok(ArraySegment<ShowDto>.Empty);
        }
        
        lastItemIndex = lastItemIndex > showsList.Length ? showsList.Length : lastItemIndex;

        var retVal = showsList[firstItemIndex..lastItemIndex];
        return Ok(retVal.Select(_showConverter.Convert));
    }
}