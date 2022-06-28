using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.App.Data.Requests;

[ExcludeFromCodeCoverage]
public sealed record PaginationQuery(int PageIndex, int ItemCount);