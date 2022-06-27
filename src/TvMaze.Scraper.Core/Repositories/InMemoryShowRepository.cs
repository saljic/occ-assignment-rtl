using System.Collections.Concurrent;
using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.Api.Repositories;

namespace TvMaze.Scraper.Core.Repositories;

public sealed class InMemoryShowRepository : IShowRepository
{
    private readonly ConcurrentDictionary<int, Show> _showDictionary = new();

    public Task AddOrUpdateAsync(Show show)
    {
        _showDictionary.AddOrUpdate(show.Id, _ => show, (_, _) => show);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Show>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Show>>(_showDictionary.Values.OrderBy(x => x.Id));
    }
}