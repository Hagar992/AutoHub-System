using AutoHub_System.Models;
using AutoHub_System.ViewModel;

namespace AutoHub_System.Services
{
    public interface IUserService : IService<User>
    {
        Task<(bool Success, string Error)> RegisterAsync(RegisterViewModel model);
        Task<(bool Success, string Error)> LoginAsync(LoginViewModel model);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> IsInRoleAsync(User user, string role);
        Task LogoutAsync();
        Task<List<User>> GetAllWithOrdersAsync();
        Task<User?> GetUserDetailsAsync(string id);
        Task<User?> GetByIdAsync(string id);
        Task<List<User>> GetAllAsync();
    }
}
