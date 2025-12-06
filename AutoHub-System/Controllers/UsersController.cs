using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoHub_System.Models;
using AutoHub_System.Services.Interfaces;


namespace AutoHub_System.Controllers
{
    

    namespace AutoHub_System.Controllers
    {
        [Authorize(Roles = "Admin")]
        public class UsersController : Controller
        {
            private readonly IUserService _userService;
            private readonly UserManager<User> _userManager;

            public UsersController(IUserService userService, UserManager<User> userManager)
            {
                _userService = userService;
                _userManager = userManager;
            }

            public async Task<IActionResult> Index()
            {
                var users = await _userService.GetAllAsync();
                return View(users);
            }

            public async Task<IActionResult> Details(string id)
            {
                var user = await _userService.GetUserDetailsAsync(id);

                if (user == null)
                    return NotFound();

                return View(user);
            }

            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> Block(string id)
            //{
            //    var user = await _userManager.FindByIdAsync(id);
            //    if (user == null)
            //        return NotFound();

            //    user.LockoutEnabled = true;
            //    user.LockoutEnd = DateTime.Now.AddYears(100);

            //    await _userManager.UpdateAsync(user);

            //    return RedirectToAction(nameof(Index));
            //}
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> ToggleBlock(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();

                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
                {
                    // Unlock
                    user.LockoutEnd = null;
                    user.LockoutEnabled = false;
                }
                else
                {
                    // Block
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.Now.AddYears(100);
                }

                await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }

        }
    }

}
