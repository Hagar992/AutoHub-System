using AutoHub_System.Data;
using AutoHub_System.Models;

namespace AutoHub_System.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;
        public ContactRepository(ApplicationDbContext context) => _context = context;

        public async Task AddMessageAsync(ContactMessage message)
        {
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
