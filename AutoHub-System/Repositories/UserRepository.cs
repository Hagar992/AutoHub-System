using AutoHub_System.Data;
using AutoHub_System.Models;
using AutoHub_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace AutoHub_System.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public bool EmailExists(string email)
        {
            return dbSet.Any(u => u.Email == email);
        }
        public async Task<User?> GetUserWithOrdersDetailsAsync(string id)
        {
            return await dbSet
                .Where(u => u.Id == id)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.Car)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.DepositePolicy)
                .Include(u => u.Orders)
                    .ThenInclude(o => o.PaymentInfo)
                .FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetAllWithOrdersAsync()
        {
            return await dbSet
                .Include(u => u.Orders)
                .ToListAsync();
        }
        public async Task<User?> GetByIdAsync(string id)
        {
            return await dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
