using AutoHub_System.Models;

namespace AutoHub_System.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        bool EmailExists(string email);
    }
}
