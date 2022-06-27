using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Refit;
using TvMaze.Scraper.Api.Repositories;
using TvMaze.Scraper.Core.External;
using TvMaze.Scraper.Core.Factories;

namespace TvMaze.Scraper.Core.Services;

public sealed class ScrapingService : IScrapingService
{
    private const int SECONDS_BEFORE_RETRY = 11;
    private readonly AsyncRetryPolicy _retryTooManyRequestsPolicy;
    private readonly IShowFactory _showFactory;
    private readonly IShowRepository _showRepository;
    private readonly ILogger<ScrapingService> _logger;
    private readonly ITvMazeApi _tvMazeApi;

    public ScrapingService(ITvMazeApi tvMazeApi, IShowFactory showFactory, IShowRepository showRepository, ILogger<ScrapingService> logger)
    {
        _tvMazeApi = tvMazeApi;
        _showFactory = showFactory;
        _showRepository = showRepository;
        _logger = logger;
        _retryTooManyRequestsPolicy = Policy
            .Handle<ApiException>(x => x.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryForeverAsync(x => TimeSpan.FromSeconds(SECONDS_BEFORE_RETRY));
    }

    public async Task ScrapeTvMaze(int startPage)
    {
        try
        {
            var tvMazeShows = await _retryTooManyRequestsPolicy
                .ExecuteAsync(async () => await _tvMazeApi.GetShows(startPage));

            if (!tvMazeShows.Any()) return;

            foreach (var tvMazeShow in tvMazeShows)
            {
                var tvMazeCast = await _retryTooManyRequestsPolicy
                     .ExecuteAsync(async () => await _tvMazeApi.GetCast(tvMazeShow.Id));
                
                var show = _showFactory.Create(tvMazeShow, tvMazeCast);
                await _showRepository.AddOrUpdateAsync(show);
            }
        }
        catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation($"{nameof(ScrapingService)} has finished scraping tv maze api!");
            return;
        }

        await ScrapeTvMaze(++startPage);
    }
}