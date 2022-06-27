using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using TvMaze.Scraper.Api.DomainModels;
using TvMaze.Scraper.App.Converters;

namespace TvMaze.Scraper.App.UnitTest.Converters;

public class ShowDataConverterTests
{
    [Fact]
    public void Convert_ThrowsInvalidOperationException()
    {
        var fixture = new Fixture();
        fixture.Register(() => DateOnly.MinValue);

        var show = fixture.Create<Show>();

        var sut = new ShowDataConverter();
        
        var showDto = sut.Convert(show);

        showDto.Should().NotBeNull();
        showDto.Id.Should().Be(show.Id);
        showDto.Name.Should().Be(show.Name);
        
        foreach (var (dto, domain) in showDto.Cast.Zip(show.Cast))
        {
            dto.Id.Should().Be(domain.Id);
            dto.Name.Should().Be(domain.Name);
            dto.Birthday.Should().Be(domain.Birthday.ToString());
        }
    }
}