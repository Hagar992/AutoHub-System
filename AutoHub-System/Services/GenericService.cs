// GenericService.cs
using AutoHub_System.Repositories;
using System.Collections.Generic;

namespace AutoHub_System.Services
{
    public class GenericService<T> : IService<T> where T : class
    {
        protected IRepository<T> repository;

        // Fix: Add the base constructor that takes IRepository<T>
        public GenericService(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public void Add(T entity)
        {
            repository.Add(entity);
            repository.Save();
        }

        public void Delete(T entity)
        {
            repository.Delete(entity);
            repository.Save();
        }

        public List<T> get_all()
        {
            return repository.get_all();
        }

        public T? find_id(int id)
        {
            return repository.find_id(id);
        }

        public void Update(T entity)
        {
            repository.Update(entity);
            repository.Save();
        }
    }
}