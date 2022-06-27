using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.Core.External.Data;

[ExcludeFromCodeCoverage]
public record struct TvMazeCastDto(TvMazeCastPersonDto Person);