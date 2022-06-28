using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;
using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.Api.Repositories;
using TvMaze.Scraper.Core.Configuration;
using TvMaze.Scraper.Core.External;
using TvMaze.Scraper.Core.External.Data;
using TvMaze.Scraper.Core.Factories;
using TvMaze.Scraper.Core.Services;

namespace TvMaze.Scraper.Core.UnitTest.Services;

public class ScrapingServiceTests
{
    [Fact]
    public async Task ScrapeTvMaze_ShouldNotCreateOrAddAnyShow_WhenTvMazeApiContainsNoShows()
    {
        var setup = Setup();
        setup.tvMazeApiMock.Setup(x => x.GetShows(It.IsAny<int>())).ReturnsAsync(Array.Empty<TvMazeShowDto>());
        
        await setup.sut.ScrapeTvMaze();
        
        setup.showFactoryMock.Verify(x => x.Create(It.IsAny<TvMazeShowDto>(), It.IsAny<IEnumerable<TvMazeCastDto>>()), Times.Never);
        setup.showRepositoryMock.Verify(x => x.AddOrUpdateAsync(It.IsAny<Show>()), Times.Never);
    }
    
    [Fact]
    public async Task ScrapeTvMaze_ShouldCatchAndHandleApiException_WhenTvMazeApiExceptionStatusCodeNotFound()
    {
        var setup = Setup();

        var apiException = await CreateApiException(HttpStatusCode.NotFound);
        setup.tvMazeApiMock.Setup(x => x.GetShows(It.IsAny<int>())).ThrowsAsync(apiException);

        var act = async () => await setup.sut.ScrapeTvMaze();

        await act.Should().NotThrowAsync<ApiException>();
        
        setup.showFactoryMock.Verify(x => x.Create(It.IsAny<TvMazeShowDto>(), It.IsAny<IEnumerable<TvMazeCastDto>>()), Times.Never);
        setup.showRepositoryMock.Verify(x => x.AddOrUpdateAsync(It.IsAny<Show>()), Times.Never);
    }
    
    [Fact]
    public async Task ScrapeTvMaze_ShouldCatchAndRetryAfter_WhenTvMazeApiExceptionStatusCodeTooManyRequests()
    {
        var setup = Setup();
        var apiException = await CreateApiException(HttpStatusCode.TooManyRequests);
        setup.tvMazeApiMock.SetupSequence(x => x.GetShows(It.IsAny<int>()))
            .ThrowsAsync(apiException)
            .ReturnsAsync(Array.Empty<TvMazeShowDto>());
        
        var act = async () => await setup.sut.ScrapeTvMaze();

        await act.Should().NotThrowAsync<ApiException>();
        
        setup.showFactoryMock.Verify(x => x.Create(It.IsAny<TvMazeShowDto>(), It.IsAny<IEnumerable<TvMazeCastDto>>()), Times.Never);
        setup.showRepositoryMock.Verify(x => x.AddOrUpdateAsync(It.IsAny<Show>()), Times.Never);
        setup.tvMazeApiMock.Verify(x => x.GetShows(It.IsAny<int>()), Times.Exactly(2));
    }
    
    [Fact]
    public async Task ScrapeTvMaze_ShouldCreateAndAddEachTvShow_WhenTvMazeApiReturnsListOfShows()
    {
        var fixture = new Fixture();
        var tvMazeApiShows = fixture.CreateMany<TvMazeShowDto>().ToArray();
        var setup = Setup();
        setup.tvMazeApiMock.SetupSequence(x => x.GetShows(It.IsAny<int>()))
            .ReturnsAsync(tvMazeApiShows)
            .ReturnsAsync(ArraySegment<TvMazeShowDto>.Empty);
        setup.tvMazeApiMock.Setup(x => x.GetCast(It.IsAny<int>())).ReturnsAsync(ArraySegment<TvMazeCastDto>.Empty);
        
        await setup.sut.ScrapeTvMaze();

        setup.showFactoryMock.Verify(x => x.Create(It.IsAny<TvMazeShowDto>(), It.IsAny<IEnumerable<TvMazeCastDto>>()), Times.Exactly(tvMazeApiShows.Length));
        setup.showRepositoryMock.Verify(x => x.AddOrUpdateAsync(It.IsAny<Show>()), Times.Exactly(tvMazeApiShows.Length));
    }
    
    [Fact]
    public async Task ScrapeTvMaze_ShouldStopRecursivelyCallingGetShowsForEachPage_WhenGetShowsReturnsEmptyList()
    {
        var fixture = new Fixture();
        var tvMazeApiShows = fixture.CreateMany<TvMazeShowDto>().ToArray();
        var setup = Setup();
        setup.tvMazeApiMock.SetupSequence(x => x.GetShows(It.IsAny<int>()))
            .ReturnsAsync(tvMazeApiShows)
            .ReturnsAsync(ArraySegment<TvMazeShowDto>.Empty);
        setup.tvMazeApiMock.Setup(x => x.GetCast(It.IsAny<int>())).ReturnsAsync(ArraySegment<TvMazeCastDto>.Empty);
        
        await setup.sut.ScrapeTvMaze();

        setup.tvMazeApiMock.Verify(x => x.GetShows(0), Times.Once);
        setup.tvMazeApiMock.Verify(x => x.GetShows(1), Times.Once);
        setup.tvMazeApiMock.Verify(x => x.GetShows(2), Times.Never);
    }
    
    [Fact]
    public async Task ScrapeTvMaze_ShouldGetCastForEachShow_WhenGetShowsReturnsShows()
    {
        var fixture = new Fixture();
        var tvMazeApiShows = fixture.CreateMany<TvMazeShowDto>().ToArray();
        var tvMazeApiCast = fixture.CreateMany<TvMazeCastDto>().ToArray();
        var setup = Setup();
        setup.tvMazeApiMock.SetupSequence(x => x.GetShows(It.IsAny<int>()))
            .ReturnsAsync(tvMazeApiShows)
            .ReturnsAsync(ArraySegment<TvMazeShowDto>.Empty);
        setup.tvMazeApiMock.Setup(x => x.GetCast(It.IsAny<int>())).ReturnsAsync(tvMazeApiCast);
        
        await setup.sut.ScrapeTvMaze();

        setup.showFactoryMock.Verify(x => x.Create(It.IsAny<TvMazeShowDto>(), tvMazeApiCast), Times.Exactly(tvMazeApiShows.Length));
    }

    private (ScrapingService sut, Mock<ITvMazeApi> tvMazeApiMock, Mock<IShowFactory> showFactoryMock, Mock<IShowRepository> showRepositoryMock) Setup()
    {
        var tvMazeApiMock = new Mock<ITvMazeApi>();
        var showFactoryMock = new Mock<IShowFactory>();
        var showRepositoryMock = new Mock<IShowRepository>();
        var sut = new ScrapingService(tvMazeApiMock.Object, showFactoryMock.Object, showRepositoryMock.Object, new ScrapingServiceConfiguration(TimeSpan.FromMilliseconds(1)), Mock.Of<ILogger<ScrapingService>>());
        return (sut, tvMazeApiMock, showFactoryMock, showRepositoryMock);
    }

    private async Task<ApiException> CreateApiException(HttpStatusCode statusCode) => await ApiException.Create(
        new HttpRequestMessage(), HttpMethod.Get, new HttpResponseMessage(statusCode),
        new RefitSettings());
}