// GenericRepository.cs
using AutoHub_System.Data;
using AutoHub_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AutoHub_System.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext db;
        protected DbSet<T> dbset;

        public GenericRepository(ApplicationDbContext context)
        {
            db = context;
            dbset = db.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public List<T> get_all()
        {
            return dbset.ToList();
        }

        public T? find_id(int id)
        {
            return dbset.Find(id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(T entity)
        {
            dbset.Update(entity);
        }
    }
}