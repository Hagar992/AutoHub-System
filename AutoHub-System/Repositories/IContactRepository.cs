using AutoHub_System.Models;

namespace AutoHub_System.Repositories
{
    public interface IContactRepository
    {
        Task AddMessageAsync(ContactMessage message);
    }
}
