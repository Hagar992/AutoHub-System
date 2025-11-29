// IRepository.cs
using System.Collections.Generic;

namespace AutoHub_System.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> get_all();
        T? find_id(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}