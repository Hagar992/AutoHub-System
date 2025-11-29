using AutoHub_System.Models;
using AutoHub_System.ViewModel;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;

namespace AutoHub_System.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Cloudinary _cloudinary;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            Cloudinary cloudinary)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cloudinary = cloudinary;
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterViewModel model)
        {
            string profileUrl = "default.png";

            // Upload to Cloudinary
            if (model.ProfileImage != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(model.ProfileImage.FileName, model.ProfileImage.OpenReadStream()),
                    Folder = "autohub/users"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                profileUrl = uploadResult.SecureUrl.ToString();
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.FullName,
                ProfilePicture = profileUrl,
                DateRegistered = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            return (true, "");
        }

        public async Task<(bool Success, string ErrorMessage)> LoginAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
                return (false, "Invalid email or password.");

            return (true, "");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
