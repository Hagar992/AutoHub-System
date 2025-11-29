using AutoHub_System.ViewModel;
namespace AutoHub_System.Services
{
    public interface IAccountService
    {
        Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterViewModel model);
        Task<(bool Success, string ErrorMessage)> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
