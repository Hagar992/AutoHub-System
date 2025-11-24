using AutoHub_System.Models;
using AutoHub_System.Services;
using AutoHub_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
namespace AutoHub_System.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ICarService _carService;
        public PaymentController(IPaymentService paymentService, ICarService carService) // تم التغيير من baseService
        {
            _paymentService = paymentService;
            _carService = carService;
        }
        public async Task<IActionResult> Index(int carId, decimal deposit)
        {
            var car = await _carService.GetByIdAsync(carId);
            if (car == null) return NotFound();

           
            deposit = car.Price * 0.1m;

            var vm = new PaymentViewModel
            {
                CarID = car.CarID,
                Deposit = deposit,
                CarBrand = car.Brand,
                CarModel = car.Model,
                Price = car.Price
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(PaymentViewModel model)
        {
            // ====== لو فيه أخطاء في الـ validation ======
            if (!ModelState.IsValid)
            {
                // نجيب السيارة تاني عشان نعبي البيانات في الـ Model ونرجعها للـ View
                var carForError = await _carService.GetByIdAsync(model.CarID);
                if (carForError != null)
                {
                    model.CarBrand = carForError.Brand;
                    model.CarModel = carForError.Model;
                    model.Price = carForError.Price;
                    model.Deposit = carForError.Price * 0.1m;
                }
                return View("Index", model);
            }

            // ====== لو كل حاجة تمام (نجيب السيارة مرة تانية للـ Stripe) ======
            var car = await _carService.GetByIdAsync(model.CarID);
            if (car == null)
            {
                ModelState.AddModelError("", "Car not found.");
                // لو السيارة مش موجودة نرجع تاني مع البيانات
                model.CarBrand = "Unknown";
                model.CarModel = "Car";
                model.Price = 0;
                model.Deposit = 0;
                return View("Index", model);
            }

            var amount = model.Deposit;
            var exchangeRate = 48.5m;
            var amountUSD = Math.Round(amount / exchangeRate, 2);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = amountUSD * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Deposit 10% - {car.Brand} {car.Model}"
                    }
                },
                Quantity = 1,
            }
        },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Payment", null, Request.Scheme) + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme),
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            // حفظ البيانات لصفحة الـ Success
            TempData["Name"] = model.Name;
            TempData["Email"] = model.Email;
            TempData["Phone"] = model.Phone;
            TempData["Phone2"] = model.Phone2;
            TempData["SSN"] = model.SSN;
            TempData["Amount"] = amount.ToString("N0");

            return Redirect(session.Url);
        }

        public async Task<IActionResult> Success(string session_id)
        {
            var service = new SessionService();
            var session = await service.GetAsync(session_id);
            if (session.PaymentStatus == "paid")
            {
                var payment = new PaymentInfo
                {
                    Name = TempData["Name"]?.ToString(),
                    Email = TempData["Email"]?.ToString(),
                    Phone = TempData["Phone"]?.ToString(),
                    Phone2 = TempData["Phone2"]?.ToString(),
                    SSN = TempData["SSN"]?.ToString(),
                    Amount = decimal.Parse(TempData["Amount"]?.ToString() ?? "0"),
                    CreatedAt = DateTime.Now
                };

                await _paymentService.SavePaymentAsync(payment);
                ViewBag.Name = payment.Name;
                ViewBag.Amount = payment.Amount.ToString("F2");
                return View("Success");
            }
            return RedirectToAction("Cancel");
        }
        public IActionResult Cancel()
        {
            return View();
        }

    }
}