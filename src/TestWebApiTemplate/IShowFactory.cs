﻿namespace TestWebApiTemplate;

public interface IShowFactory
{
    Show Create(TvMazeShowDto showDto, IEnumerable<TvMazeCastDto> castDto);
}

public sealed class ShowFactory : IShowFactory
{
    public Show Create(TvMazeShowDto showDto, IEnumerable<TvMazeCastDto> castDto)
    {
        var cast = castDto
            .Select(x => new CastMember(x.Person.Id, x.Person.Name,
                x.Person.Birthday == null ? null : DateOnly.Parse(x.Person.Birthday)))
            .OrderByDescending(x => x.Birthday)
            .ToArray();
        return new Show(showDto.Id, showDto.Name, cast);
    }
}