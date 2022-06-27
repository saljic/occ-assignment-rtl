using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.Core.External.Data;

[ExcludeFromCodeCoverage]
public record struct TvMazeCastPersonDto(int Id, string Name, string? Birthday);