namespace TestWebApiTemplate;

public interface IShowRepository
{
    Task AddOrUpdateAsync(Show show);
    Task<IEnumerable<Show>> GetAllAsync();
}