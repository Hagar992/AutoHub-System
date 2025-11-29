namespace AutoHub_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

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

            //TempData["SuccessMessage"] = "Your account has been created successfully! Please log in.";
            //return RedirectToAction("Login");
            TempData["SuccessMessage"] = "Your account has been created successfully! Please log in.";
            return RedirectToAction("Login");////////////////ال home اسمها اييييييي
        }

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
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login", "Account");
        }


   
    }
}
