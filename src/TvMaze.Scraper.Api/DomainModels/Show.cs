namespace TvMaze.Scraper.Api.DomainModels;

public sealed class Show
{
    public Show(int id, string name, IEnumerable<CastMember> cast)
    {
        Id = id;
        Name = name;
        Cast = cast;
    }

    public int Id { get; }
    public string Name { get; }
    public IEnumerable<CastMember> Cast { get; }
}