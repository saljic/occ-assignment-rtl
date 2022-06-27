using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.App.Data.Dtos;

[ExcludeFromCodeCoverage]
public record struct ShowDto(int Id, string Name, IEnumerable<CastMemberDto> Cast);