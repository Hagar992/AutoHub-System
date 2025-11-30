[AllowAnonymous]
public class CarsController : BaseController
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService, UserManager<User> userManager)
        : base(userManager)
    {
        _carService = carService;
    }

    public IActionResult Index()
    {
        var cars = _carService.get_all(); // Use synchronous version
        return View(cars);
    }

    public async Task<IActionResult> CarDetails(int id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car == null)
            return NotFound();
        return View(car);
    }
}