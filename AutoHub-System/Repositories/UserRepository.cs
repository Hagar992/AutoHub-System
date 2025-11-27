using AutoHub_System.Data;
using AutoHub_System.Models;
using AutoHub_System.Repositories.Interfaces;



namespace AutoHub_System.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public bool EmailExists(string email)
        {
            return dbSet.Any(u => u.Email == email);
        }
    }
}
