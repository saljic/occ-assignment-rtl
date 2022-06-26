namespace TestWebApiTemplate;

public record struct ShowDto(int Id, string Name, IEnumerable<CastMemberDto> Cast);