﻿using System.Net;
using Polly;
using Polly.Retry;
using Refit;

namespace TestWebApiTemplate;

public sealed class ScrapingService : IScrapingService
{
    private const int SECONDS_BEFORE_RETRY = 11;
    private readonly AsyncRetryPolicy _retryTooManyRequestsPolicy;
    private readonly IShowFactory _showFactory;
    private readonly IShowRepository _showRepository;
    private readonly ITvMazeApi _tvMazeApi;

    public ScrapingService(ITvMazeApi tvMazeApi, IShowFactory showFactory, IShowRepository showRepository)
    {
        _tvMazeApi = tvMazeApi;
        _showFactory = showFactory;
        _showRepository = showRepository;
        _retryTooManyRequestsPolicy = Policy
            .Handle<ApiException>(x => x.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryForeverAsync(x => TimeSpan.FromSeconds(SECONDS_BEFORE_RETRY));
    }

    public async Task ScrapeTvMaze(int startPage)
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

        await ScrapeTvMaze(++startPage);
    }
}