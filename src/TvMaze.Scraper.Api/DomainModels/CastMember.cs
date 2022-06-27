using System.Diagnostics.CodeAnalysis;

namespace TvMaze.Scraper.Api.DomainModels;

/// <summary>
///     The <see cref="CastMember" /> contains information about a cast member
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CastMember
{
    public CastMember(int id, string name, DateOnly? birthday)
    {
        Id = id;
        Name = name;
        Birthday = birthday;
    }

    /// <summary>
    ///     Gets the unique identifier of the cast member
    /// </summary>
    public int Id { get; }

    /// <summary>
    ///     Gets the name of the cast member
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the birthday of the cast member
    ///     <remarks>
    ///         Birthday is null when the birthday is unknown
    ///     </remarks>
    /// </summary>
    public DateOnly? Birthday { get; }
}