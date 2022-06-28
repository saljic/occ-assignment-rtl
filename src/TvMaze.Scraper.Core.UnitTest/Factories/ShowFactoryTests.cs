using AutoFixture;
using FluentAssertions;
using TvMaze.Scraper.Core.External.Data;
using TvMaze.Scraper.Core.Factories;

namespace TvMaze.Scraper.Core.UnitTest.Factories;

public class ShowFactoryTests
{
    [Fact]
    public void Create_ShouldCreateShow_WithSameValuesAsDto()
    {
        var fixture = new Fixture();
        var showDto = fixture.Create<TvMazeShowDto>();
        var castMemberDto = new TvMazeCastDto(new TvMazeCastPersonDto(fixture.Create<int>(), fixture.Create<string>(), "1997-01-02"));
        
        var sut = new ShowFactory();

        var show = sut.Create(showDto, new [] {castMemberDto});
        
        show.Id.Should().Be(showDto.Id);
        show.Name.Should().Be(showDto.Name);
        show.Cast.Should().ContainSingle();
        
        var castMember = show.Cast.First();
        castMember.Id.Should().Be(castMemberDto.Person.Id);
        castMember.Name.Should().Be(castMemberDto.Person.Name);
        castMember.Birthday.Should().Be(DateOnly.Parse(castMemberDto.Person.Birthday!));
    }
    
    [Fact]
    public void Create_ShouldCreateShow_WithCastOrderedByBirthdayDescending()
    {
        var fixture = new Fixture();
        var showDto = fixture.Create<TvMazeShowDto>();
        var castMemberDto1 = new TvMazeCastDto(new TvMazeCastPersonDto(fixture.Create<int>(), fixture.Create<string>(), "1997-01-02"));
        var castMemberDto2 = new TvMazeCastDto(new TvMazeCastPersonDto(fixture.Create<int>(), fixture.Create<string>(), "1997-01-03"));
        var castMemberDto3 = new TvMazeCastDto(new TvMazeCastPersonDto(fixture.Create<int>(), fixture.Create<string>(), "1997-01-01"));
        
        var sut = new ShowFactory();

        var show = sut.Create(showDto, new [] {castMemberDto1, castMemberDto2, castMemberDto3});

        var castMember = show.Cast.ToArray();
        castMember[0].Birthday!.Value.Day.Should().Be(3);
        castMember[1].Birthday!.Value.Day.Should().Be(2);
        castMember[2].Birthday!.Value.Day.Should().Be(1);
    }
    
    [Fact]
    public void Create_CastMemberBirthdayNull_SetsBirthdayToNull()
    {
        var fixture = new Fixture();
        var showDto = fixture.Create<TvMazeShowDto>();
        var castMemberDto = new TvMazeCastDto(new TvMazeCastPersonDto(fixture.Create<int>(), fixture.Create<string>(), null));
        
        var sut = new ShowFactory();

        var show = sut.Create(showDto, new [] {castMemberDto});

        show.Cast.Should().ContainSingle();
        show.Cast.First().Birthday.Should().BeNull();
    }
}