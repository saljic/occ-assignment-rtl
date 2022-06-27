namespace TestWebApiTemplate;

public sealed class ShowConverter : IShowConverter
{
    public ShowDto Convert(Show show)
    {
        var castDto = show.Cast.Select(x => new CastMemberDto(x.Id, x.Name, x.Birthday.ToString()));
        return new ShowDto(show.Id, show.Name, castDto);
    }
}