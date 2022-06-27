using FluentAssertions;
using TvMaze.Scraper.App.Converters;

namespace TvMaze.Scraper.App.UnitTest.Converters;

public class DefaultDataConverterTests
{
    [Fact]
    public void Convert_ThrowsInvalidOperationException()
    {
        var sut = new DefaultDataConverter<object,object>();
        
        var act = () => sut.Convert(new object());

        act.Should().ThrowExactly<InvalidOperationException>();
    }
}