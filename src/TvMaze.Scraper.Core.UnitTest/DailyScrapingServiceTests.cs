using Microsoft.Extensions.Logging;
using Microsoft.Reactive.Testing;
using Moq;
using TvMaze.Scraper.Core.Services;

namespace TvMaze.Scraper.Core.UnitTest;

public class DailyScrapingServiceTests
{
    [Fact]
    public async Task StartAsync_ShouldScrapeTvMazeApiImmediately()
    {
        var testScheduler = new TestScheduler();
        var scrapingServiceMock = new Mock<IScrapingService>();
        var sut = new DailyScrapingService(scrapingServiceMock.Object, testScheduler, Mock.Of<ILogger<DailyScrapingService>>());

        await sut.StartAsync(CancellationToken.None);
        
        scrapingServiceMock.Verify(x => x.ScrapeTvMaze(), Times.Once);
    }
    
    [Fact]
    public async Task StartAsync_ShouldScrapeTvMazeApiDaily()
    {
        var testScheduler = new TestScheduler();
        var scrapingServiceMock = new Mock<IScrapingService>();
        var sut = new DailyScrapingService(scrapingServiceMock.Object, testScheduler, Mock.Of<ILogger<DailyScrapingService>>());

        await sut.StartAsync(CancellationToken.None);
        
        scrapingServiceMock.Verify(x => x.ScrapeTvMaze(), Times.Once);
        
        testScheduler.AdvanceBy(TimeSpan.FromDays(1).Ticks);
        
        scrapingServiceMock.Verify(x => x.ScrapeTvMaze(), Times.Exactly(2));
    }
    
    [Fact]
    public async Task StartAsync_ShouldStopScrapingTvMazeApiDaily_WhenCancellationTokenCancelled()
    {
        var testScheduler = new TestScheduler();
        var scrapingServiceMock = new Mock<IScrapingService>();
        var sut = new DailyScrapingService(scrapingServiceMock.Object, testScheduler, Mock.Of<ILogger<DailyScrapingService>>());

        var tokenSource = new CancellationTokenSource();
        
        await sut.StartAsync(tokenSource.Token);
        
        scrapingServiceMock.Verify(x => x.ScrapeTvMaze(), Times.Once);

        tokenSource.Cancel();
        
        testScheduler.AdvanceBy(TimeSpan.FromDays(1).Ticks);
        
        scrapingServiceMock.Verify(x => x.ScrapeTvMaze(), Times.Once);
    }
    
    [Fact]
    public async Task StartAsync_ShouldKeepScrapingDaily_WhenScrapingServiceThrowsException()
    {
        var testScheduler = new TestScheduler();
        var scrapingServiceMock = new Mock<IScrapingService>();
        scrapingServiceMock.Setup(x => x.ScrapeTvMaze()).Throws<InvalidOperationException>();
        
        var sut = new DailyScrapingService(scrapingServiceMock.Object, testScheduler, Mock.Of<ILogger<DailyScrapingService>>());

        await sut.StartAsync(CancellationToken.None);
        
        scrapingServiceMock.Verify(x => x.ScrapeTvMaze(), Times.Once);
        
        testScheduler.AdvanceBy(TimeSpan.FromDays(1).Ticks);
        
        scrapingServiceMock.Verify(x => x.ScrapeTvMaze(), Times.Exactly(2));
    }
}