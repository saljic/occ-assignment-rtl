namespace TestWebApiTemplate;

public sealed class CastMember
{
    public CastMember(int id, string name, DateOnly? birthday)
    {
        Id = id;
        Name = name;
        Birthday = birthday;
    }

    public int Id { get; }
    public string Name { get; }
    public DateOnly? Birthday { get; }
}