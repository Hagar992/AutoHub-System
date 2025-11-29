using AutoHub_System.Repositories;

namespace AutoHub_System.Services
{
    public class Service<T> : IService<T> where T : class
    {
        protected IRepository<T> _repo;
        
        public Service(IRepository<T> repo)
        {
            _repo = repo;
        }

        public List<T> get_all()
        {
            return _repo.get_all();
        }

        public T? find_id(int id)
        {
            return _repo.find_id(id);
        }

        public void Add(T entity)
        {
            _repo.Add(entity);
            _repo.Save();
        }

        public void Update(T entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public void Delete(T entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }
    }
}