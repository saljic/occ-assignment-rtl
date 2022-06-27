using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.Core.External.Data;

[ExcludeFromCodeCoverage]
public record struct TvMazeShowDto(int Id, string Name);