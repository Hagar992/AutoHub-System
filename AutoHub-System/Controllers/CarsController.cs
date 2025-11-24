using Microsoft.AspNetCore.Mvc;
using AutoHub_System.Services;
using AutoHub_System.Models;
namespace AutoHub_System.Controllers
{
    [Route("/{Controller}/{action=index}/{id?}")]
    public class CarsController : Controller
    {
        IBaseService<Car> cs;
        public CarsController(IBaseService<Car> _cs)
        {
            cs = _cs;
        }
        public IActionResult Index()
        {
            var cars = cs.get_all();
            return View(cars);
        }
       
        public IActionResult CarDetails(int id)
        {
            var car = cs.find_id(id);
            if (car == null)
                return NotFound();
            return View(car);
        }
    }
}
