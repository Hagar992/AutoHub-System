using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub_System.Controllers
{
    public class SetupController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public SetupController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Create Roles
        public async Task<IActionResult> CreateRoles()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            return Content("Roles Created Successfully");
        }

        // Create First Admin
        public async Task<IActionResult> CreateAdmin()
        {
            var adminEmail = "admin@system.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    Name = "System Admin",
                    Address = "HQ",
                    DateRegistered = DateTime.Now
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");

                if (!result.Succeeded)
                    return Content("Failed: " + string.Join(",", result.Errors.Select(e => e.Description)));
            }

            if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
                await _userManager.AddToRoleAsync(adminUser, "Admin");

            return Content("Admin Created Successfully");
        }
    }
}
