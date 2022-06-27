namespace TvMaze.Scraper.App.Data.Dtos;

public record struct ShowDto(int Id, string Name, IEnumerable<CastMemberDto> Cast);