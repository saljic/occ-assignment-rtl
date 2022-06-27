using Microsoft.AspNetCore.Mvc;
using TvMaze.Scraper.Api.DomainModels;
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
    private readonly IDataConverter<Show, ShowDto> _showDataConverter;
    private readonly IShowRepository _showRepository;

    public ShowsController(IShowRepository showRepository, IDataConverter<Show, ShowDto> showDataConverter)
    {
        _showRepository = showRepository;
        _showDataConverter = showDataConverter;
    }

    /// <summary>
    ///     Returns a paginated list of <see cref="ShowDto" />
    ///     <example>
    ///         /api/shows?PageIndex=1&ItemCount=10
    ///     </example>
    /// </summary>
    [HttpGet(Name = "GetShows")]
    public async Task<IActionResult> Get([FromQuery] PaginationQuery paginationQuery)
    {
        if (paginationQuery.PageIndex < 0 || paginationQuery.ItemCount <= 0)
            return BadRequest(ArraySegment<ShowDto>.Empty);

        var shows = await _showRepository.GetAllAsync();

        return Ok(shows
            .AsQueryable()
            .OrderBy(x => x.Id)
            .Skip(paginationQuery.PageIndex * paginationQuery.ItemCount)
            .Take(paginationQuery.ItemCount)
            .Select(_showDataConverter.Convert)
            .ToArray());
    }
}