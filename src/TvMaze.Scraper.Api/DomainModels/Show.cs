using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.Api.DomainModels;

/// <summary>
///     The <see cref="Show" /> is contains information about a tv show
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class Show
{
    public Show(int id, string name, IEnumerable<CastMember> cast)
    {
        Id = id;
        Name = name;
        Cast = cast;
    }

    /// <summary>
    ///     Gets the unique identifier of the show
    /// </summary>
    public int Id { get; }

    /// <summary>
    ///     Gets the name of the show
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets a list of all the cast members
    /// </summary>
    public IEnumerable<CastMember> Cast { get; }
}