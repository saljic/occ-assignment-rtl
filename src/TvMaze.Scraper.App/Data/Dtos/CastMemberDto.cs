using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.App.Data.Dtos;

[ExcludeFromCodeCoverage]
public record struct CastMemberDto(int Id, string Name, string? Birthday);