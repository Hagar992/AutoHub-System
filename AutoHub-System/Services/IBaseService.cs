// IBaseService.cs
namespace AutoHub_System.Services
{
    public interface IBaseService<T> where T : class
    {
        List<T> get_all();
        T? find_id(int id);

        
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
    }
}