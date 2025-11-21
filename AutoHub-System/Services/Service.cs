using AutoHub_System.Models;
using AutoHub_System.Repositories;
namespace AutoHub_System.Services
{
    public class Service<T> : IService<T> where T : class
    {
        protected IRepository<T> repo;
        public Service(IRepository<T> _repo)
        {
            repo = _repo;
        }
        public List<T> get_all()
        {
            return repo.get_all();
        }
        public T? find_id(int id)
        {
            return repo.find_id(id);
        }

     
    }
}
