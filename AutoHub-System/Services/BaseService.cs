// BaseService.cs
using AutoHub_System.Repositories;

namespace AutoHub_System.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IRepository<T> repo;

        public BaseService(IRepository<T> _repo)
        {
            repo = _repo;
        }

        public List<T> get_all() => repo.get_all();

        public T? find_id(int id) => repo.find_id(id);

        
        public async Task<List<T>> GetAllAsync() => await Task.FromResult(repo.get_all());

        public async Task<T?> GetByIdAsync(int id) => await Task.FromResult(repo.find_id(id));
    }
}