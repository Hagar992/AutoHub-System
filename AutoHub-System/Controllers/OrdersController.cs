using Microsoft.EntityFrameworkCore;
using AutoHub_System.Models;
using AutoHub_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController :  BaseController
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context, UserManager<User> userManager)
        : base(userManager)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Car)
                .Include(o => o.DepositePolicy)
                .Select(o => new OrderDashbardVM
                {
                    OrderID = o.OrderID,
                    Client = o.User.UserName,
                    UserEmail = o.User.Email,
                    CarPrice = o.PriceWhenBook,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    Deposit = o.PriceWhenBook * (float)o.DepositePolicy.DepositeRate,
                    TotalPaid = CalculateTotalPrice(o.Status, o.PriceWhenBook, (double)o.DepositePolicy.DepositeRate),
                    Car = $"{o.Car.Brand} {o.Car.Model}",
                    PolicyRate = (double)o.DepositePolicy.DepositeRate
                })
                .OrderByDescending(o => o.Status == OrderStatus.Pending)
                .ThenByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Car)
                .Include(o => o.DepositePolicy)
                .Include(o=> o.PaymentInfo)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            var order = await _context.Orders
                .Include(o => o.DepositePolicy)
                .Include(o => o.Car)
                                                                 
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = OrderStatus.Confirmed;
            order.TotalPaid = order.PriceWhenBook;
            order.BuyingDate = DateTime.Now;
            order.Car.Quantity -= 1;

            _context.Update(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Order #{order.OrderID} confirmed successfully!";
            return RedirectToAction(nameof(Index));
        }      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _context.Orders
                .Include(o => o.DepositePolicy)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = OrderStatus.Canceled;
            order.TotalPaid = order.PriceWhenBook * (float)order.DepositePolicy.DepositeRate;

            _context.Update(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Order #{order.OrderID} canceled successfully!";
            return RedirectToAction(nameof(Index));
        }

        private static float CalculateTotalPrice(string status, float priceWhenBook, double depositeRate)
        {
            return status switch
            {
                OrderStatus.Confirmed => priceWhenBook,
                OrderStatus.Canceled => priceWhenBook * (float)depositeRate,
                OrderStatus.Pending => priceWhenBook * (float)depositeRate,
                _ => priceWhenBook * (float)depositeRate
            };
        }
    }
}