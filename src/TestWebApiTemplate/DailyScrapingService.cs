using System.Reactive;
using System.Reactive.Linq;

namespace TestWebApiTemplate;

public sealed class DailyScrapingService : BackgroundService
{
    private readonly ILogger<DailyScrapingService> _logger;
    private readonly IScrapingService _scrapingService;

    public DailyScrapingService(IScrapingService scrapingService, ILogger<DailyScrapingService> logger)
    {
        _scrapingService = scrapingService;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Observable.Interval(TimeSpan.FromDays(1))
            .Select(_ => Unit.Default)
            .StartWith(Unit.Default)
            .Select(x => Observable.FromAsync(async () => await _scrapingService.ScrapeTvMaze(0))
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