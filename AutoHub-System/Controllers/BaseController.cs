public class BaseController : Controller
{
    private readonly UserManager<User> _userManager;

    public BaseController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var user = _userManager.GetUserAsync(User).Result;
        ViewBag.UserName = user?.Name ?? "Guest";
        ViewBag.ProfilePicture = !string.IsNullOrEmpty(user?.ProfilePicture)
                                 ? user.ProfilePicture
                                 : "https://res.cloudinary.com/dmsmksagp/image/upload/v1764107340/profiles/default.png";
    }
}
//this Controller only for profile picture 