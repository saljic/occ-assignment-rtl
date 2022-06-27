using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.Api.Repositories;
using TvMaze.Scraper.App.Controllers;
using TvMaze.Scraper.App.Converters;
using TvMaze.Scraper.App.Data.Dtos;
using TvMaze.Scraper.App.Data.Requests;

namespace TvMaze.Scraper.App.UnitTest.Controllers;

public class ShowsControllerTests
{
    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(0, 0)]
    public async Task Get_PageIndexLessThenZeroOrItemCountZeroOrLess_ReturnsBadRequestObjectResult(int pageIndex, int itemCount)
    {
        var showRepositoryMock = new Mock<IShowRepository>();
        var dataConverterMock = new Mock<IDataConverter<Show, ShowDto>>();
        
        var sut = new ShowsController(showRepositoryMock.Object, dataConverterMock.Object);

        var actionResult = await sut.Get(new PaginationQuery(pageIndex, itemCount));

        actionResult.Should().BeOfType<BadRequestObjectResult>();
        actionResult.As<BadRequestObjectResult>().StatusCode.Should().Be(400);
        actionResult.As<BadRequestObjectResult>().Value.Should().Be(ArraySegment<ShowDto>.Empty);
    }

    [Fact]
    public async Task Get_TwentyItemsPageIndexZeroAndItemCountTen_ReturnsFirsTenItems()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.MinValue);
        var showDataConverter = new ShowDataConverter();

        var shows = fixture.CreateMany<Show>(20).ToArray();

        var showRepositoryMock = new Mock<IShowRepository>();
        showRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(shows);

        var sut = new ShowsController(showRepositoryMock.Object, showDataConverter);

        var actionResult = await sut.Get(new PaginationQuery(0, 10));

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().StatusCode.Should().Be(200);
        actionResult.As<OkObjectResult>().Value.Should().BeOfType<ShowDto[]>();
        var actualShowDtos = actionResult.As<OkObjectResult>().Value.As<ShowDto[]>();

        actualShowDtos.Length.Should().Be(10);
        var expectedShowDtos = shows.OrderBy(x => x.Id).Take(10).Select(x => x.Id);
        actualShowDtos.Select(x => x.Id).Should().Equal(expectedShowDtos);
    }
    
    [Fact]
    public async Task Get_TwentyItemsPageIndexOneAndItemCountFive_SkipsFirstFiveItemsAndReturnsNextFive()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.MinValue);
        var showDataConverter = new ShowDataConverter();

        var shows = fixture.CreateMany<Show>(20).ToArray();

        var showRepositoryMock = new Mock<IShowRepository>();
        showRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(shows);

        var sut = new ShowsController(showRepositoryMock.Object, showDataConverter);

        var actionResult = await sut.Get(new PaginationQuery(1, 5));

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().StatusCode.Should().Be(200);
        actionResult.As<OkObjectResult>().Value.Should().BeOfType<ShowDto[]>();
        var actualShowDtos = actionResult.As<OkObjectResult>().Value.As<ShowDto[]>();

        actualShowDtos.Length.Should().Be(5);
        var expectedShowDtos = shows.OrderBy(x => x.Id).Skip(5).Take(5).Select(x => x.Id);
        actualShowDtos.Select(x => x.Id).Should().Equal(expectedShowDtos);
    }
    
    [Fact]
    public async Task Get_TwentyItemsPageIndexThreeAndItemCountSix_ReturnsLastTwoItems()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.MinValue);
        var showDataConverter = new ShowDataConverter();

        var shows = fixture.CreateMany<Show>(20).ToArray();

        var showRepositoryMock = new Mock<IShowRepository>();
        showRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(shows);

        var sut = new ShowsController(showRepositoryMock.Object, showDataConverter);

        var actionResult = await sut.Get(new PaginationQuery(3, 6));

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().StatusCode.Should().Be(200);
        actionResult.As<OkObjectResult>().Value.Should().BeOfType<ShowDto[]>();
        var actualShowDtos = actionResult.As<OkObjectResult>().Value.As<ShowDto[]>();

        actualShowDtos.Length.Should().Be(2);
        var expectedShowDtos = shows.OrderBy(x => x.Id).TakeLast(2).Select(x => x.Id);
        actualShowDtos.Select(x => x.Id).Should().Equal(expectedShowDtos);
    }
    
    [Fact]
    public async Task Get_PageIndexLargerThenList_ReturnsEmptyList()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.MinValue);
        var showDataConverter = new ShowDataConverter();

        var shows = fixture.CreateMany<Show>(20).ToArray();

        var showRepositoryMock = new Mock<IShowRepository>();
        showRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(shows);

        var sut = new ShowsController(showRepositoryMock.Object, showDataConverter);

        var actionResult = await sut.Get(new PaginationQuery(20, 5));

        actionResult.Should().BeOfType<OkObjectResult>();
        actionResult.As<OkObjectResult>().StatusCode.Should().Be(200);
        actionResult.As<OkObjectResult>().Value.Should().BeOfType<ShowDto[]>();
        var actualShowDtos = actionResult.As<OkObjectResult>().Value.As<ShowDto[]>();

        actualShowDtos.Length.Should().Be(0);
    }
    
}