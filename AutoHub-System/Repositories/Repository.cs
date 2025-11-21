
using AutoHub_System.Data;
using AutoHub_System.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoHub_System.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext db;
        protected DbSet<T> dbSet;
        public Repository(ApplicationDbContext _db)
        {
            db = _db;
            dbSet = db.Set<T>(); 
        }
      

        public List<T> get_all()
        {
            return dbSet.ToList();
        }
        public T? find_id(int id)
        {
            return dbSet.Find(id);
        }
    }
}
