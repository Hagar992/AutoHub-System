using AutoHub_System.Models;
using AutoHub_System.Services;
using AutoHub_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutoHub_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService _carService;
        private readonly IContactService _contactService;

        public HomeController(ICarService carService, IContactService contactService)
        {
            _carService = carService;
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel
            {
                MostPickedCars = await _carService.GetRandomMostPickedCarsAsync(6)
            };
            return View(model);
        }

        public IActionResult Privacy() => View();

        public IActionResult Terms() => View();

        public IActionResult Contact() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(string Name, string Email, string Message)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Please fill all fields correctly.";
                return View();
            }

            await _contactService.SendMessageAsync(Name, Email, Message);

            ViewData["Success"] = $"Thank you {Name}! Your message has been sent successfully. We'll reply to {Email} soon.";
            ModelState.Clear(); 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}