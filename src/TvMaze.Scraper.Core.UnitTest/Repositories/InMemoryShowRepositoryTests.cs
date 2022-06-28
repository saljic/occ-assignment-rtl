using AutoFixture;
using FluentAssertions;
using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.Core.Repositories;

namespace TvMaze.Scraper.Core.UnitTest.Repositories;

public class InMemoryShowRepositoryTests
{
    [Fact]
    public async Task AddOrUpdateAsync_ShowDoesntExist_AddsShowToDictionary()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.MinValue);
        var show = fixture.Create<Show>();
        
        var sut = new InMemoryShowRepository();

        var items = await sut.GetAllAsync();
        items.Should().BeEmpty();

        await sut.AddOrUpdateAsync(show);
        
        items = await sut.GetAllAsync();

        items.Should().ContainSingle();
        items.First().Should().Be(show);
    }
    
    [Fact]
    public async Task AddOrUpdateAsync_ShowExistsWithSameId_UpdatesShow()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.MinValue);
        var show = fixture.Create<Show>();
        var updatedShow = new Show(show.Id, fixture.Create<string>(), new CastMember[] { });
        
        var sut = new InMemoryShowRepository();

        var items = await sut.GetAllAsync();
        items.Should().BeEmpty();

        await sut.AddOrUpdateAsync(show);
        
        items = await sut.GetAllAsync();

        items.Should().ContainSingle();
        items.First().Should().Be(show);
        
        await sut.AddOrUpdateAsync(updatedShow);

        items = await sut.GetAllAsync();
        
        items.Should().ContainSingle();
        items.First().Should().NotBe(show);
        items.First().Should().Be(updatedShow);
    }
}