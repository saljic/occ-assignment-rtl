using System.Net;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Refit;
using TvMaze.Scraper.Api.Repositories;
using TvMaze.Scraper.Core.Configuration;
using TvMaze.Scraper.Core.External;
using TvMaze.Scraper.Core.External.Data;
using TvMaze.Scraper.Core.Factories;

namespace TvMaze.Scraper.Core.Services;

public sealed class ScrapingService : IScrapingService
{
    private readonly ILogger<ScrapingService> _logger;
    private readonly AsyncRetryPolicy _retryTooManyRequestsPolicy;
    private readonly IShowFactory _showFactory;
    private readonly IShowRepository _showRepository;
    private readonly ITvMazeApi _tvMazeApi;

    public ScrapingService(ITvMazeApi tvMazeApi, IShowFactory showFactory, IShowRepository showRepository, ScrapingServiceConfiguration scrapingServiceConfiguration, ILogger<ScrapingService> logger)
    {
        _tvMazeApi = tvMazeApi;
        _showFactory = showFactory;
        _showRepository = showRepository;
        _logger = logger;
        _retryTooManyRequestsPolicy = Policy
            .Handle<ApiException>(x => x.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryForeverAsync(_ => scrapingServiceConfiguration.TimeToWaitBeforeRetryTooManyRequests);
    }

    public async Task ScrapeTvMaze() => await ScrapeTvMazeRecursively(0);

    private async Task ScrapeTvMazeRecursively(int page)
    {
        try
        {
            var tvMazeShows = await GetShows(page);

            if (!tvMazeShows.Any()) return;

            foreach (var tvMazeShow in tvMazeShows)
            {
                var tvMazeCast = await GetCast(tvMazeShow.Id);

                var show = _showFactory.Create(tvMazeShow, tvMazeCast);
                
                await _showRepository.AddOrUpdateAsync(show);
            }
        }
        catch (ApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation($"{nameof(ScrapingService)} has finished scraping tv maze api!");
            return;
        }

        await ScrapeTvMazeRecursively(++page);
    }

    private async Task<IReadOnlyCollection<TvMazeCastDto>> GetCast(int showId) => await _retryTooManyRequestsPolicy
        .ExecuteAsync(async () => await _tvMazeApi.GetCast(showId));

    private async Task<IReadOnlyCollection<TvMazeShowDto>> GetShows(int page) => await _retryTooManyRequestsPolicy
        .ExecuteAsync(async () => await _tvMazeApi.GetShows(page));
}