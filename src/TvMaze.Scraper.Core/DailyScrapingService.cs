using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TvMaze.Scraper.Core.Services;

namespace TvMaze.Scraper.Core;

/// <summary>
///     The <see cref="DailyScrapingService" /> background service will run on startup the of application and scrape the tv
///     maze api on a daily basis
/// </summary>
public sealed class DailyScrapingService : BackgroundService
{
    private readonly ILogger<DailyScrapingService> _logger;
    private readonly IScrapingService _scrapingService;
    private readonly IScheduler _scheduler;

    public DailyScrapingService(IScrapingService scrapingService, IScheduler scheduler, ILogger<DailyScrapingService> logger)
    {
        _scrapingService = scrapingService;
        _scheduler = scheduler;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Observable.Interval(TimeSpan.FromDays(1), _scheduler)
            .Select(_ => Unit.Default)
            .StartWith(Unit.Default)
            .Select(x => Observable.FromAsync(async () => await _scrapingService.ScrapeTvMaze())
                .Catch<Unit, Exception>(ex =>
                {
                    _logger.LogError(ex, "Exception occured while scraping tv maze api!");
                    return Observable.Return(Unit.Default);
                }))
            .Concat()
            .Subscribe(stoppingToken);

        return Task.CompletedTask;
    }
}