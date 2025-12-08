using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoHub_System.Models;
using AutoHub_System.Services;
using AutoHub_System.Services.Interfaces;

namespace AutoHub_System.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            IUserService userService,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context):base(userManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // ==========================================
        // REGISTER
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (Success, Error) = await _userService.RegisterAsync(model);

            if (!Success)
            {
                ModelState.AddModelError(nameof(model.Email), Error);
                return View(model);
            }

            TempData["SuccessMessage"] = "Your account has been created successfully! Please log in.";
            return RedirectToAction("Login");
        }

        // ==========================================
        // LOGIN
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.LoginAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error);
                return View(model);
            }

            var user = await _userService.GetByEmailAsync(model.Email);

            // Check role
            if (await _userService.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // LOGOUT - POST Method for Security
        // ==========================================
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // USER PROFILE
        // ==========================================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Get all user orders with related data
            var orders = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.DepositePolicy)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new UserOrderViewModel
                {
                    OrderID = o.OrderID,
                    CarName = $"{o.Car.Brand} {o.Car.Model}",
                    CarImage = o.Car.MainImage,
                    CarPrice = o.PriceWhenBook,
                    Deposit = o.PriceWhenBook * (float)o.DepositePolicy.DepositeRate,
                    TotalPaid = o.TotalPaid,
                    DepositRate = (double)o.DepositePolicy.DepositeRate,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    BuyingDate = o.BuyingDate,
                    CarID = o.Car.CarID
                })
                .ToListAsync();

            // Create view model with user data and statistics
            var viewModel = new UserProfileViewModel
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                ProfilePicture = user.ProfilePicture,
                DateRegistered = user.DateRegistered,

                // Calculate statistics
                TotalOrders = orders.Count,
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                ConfirmedOrders = orders.Count(o => o.Status == OrderStatus.Confirmed),
                CanceledOrders = orders.Count(o => o.Status == OrderStatus.Canceled),
                TotalSpent = orders.Where(o => o.Status == OrderStatus.Confirmed).Sum(o => o.TotalPaid),

                Orders = orders
            };

            return View(viewModel);
        }

        
    }
}