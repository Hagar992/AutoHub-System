using AutoHub_System.Models;
using AutoHub_System.Repositories;
using AutoHub_System.Services.Interfaces;
using AutoHub_System.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace AutoHub_System.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICloudinaryService _cloudinaryService;

        public UserService(
            IRepository<User> repo,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICloudinaryService cloudinaryService
        ) : base(repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<(bool Success, string Error)> RegisterAsync(RegisterViewModel model)
        {
            // Check if email is already used
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return (false, "This email is already in use.");
            }

            // Upload profile image if provided
            string profileUrl = "default.png";
            if (model.ProfileImage != null)
            {
                profileUrl = await _cloudinaryService.UploadImageAsync(model.ProfileImage, "profiles");
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.FullName,
                Address = "Default address",
                ProfilePicture = profileUrl
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return (false, "Registration failed. Please check your input.");

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            return (true, "");
        }

        public async Task<(bool Success, string Error)> LoginAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
                return (false, "Email or password is incorrect");

            return (true, "");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> IsInRoleAsync(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
