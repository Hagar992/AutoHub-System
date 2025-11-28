[Authorize(Roles = "Admin")]
public class DashboardController : BaseController
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context, UserManager<User> userManager)
        : base(userManager)
    {
        _context = context;
    }
   
    public IActionResult Index()
    {
        MainDashbardViewModel mainData = new()
        {
            TotalNumberOfCars = _context.Cars.Count(),
            TotalDeposits = _context.Orders.Sum(o => o.PriceWhenBook),
            ActiveReservations = _context.Orders.Count(o => o.Status == "Confirmed")
        };

        return View(mainData);
    }
}
