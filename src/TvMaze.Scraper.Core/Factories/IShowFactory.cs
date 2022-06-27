namespace TestWebApiTemplate;

public interface IShowFactory
{
    Show Create(TvMazeShowDto showDto, IEnumerable<TvMazeCastDto> castDto);
}