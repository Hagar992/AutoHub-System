using AutoHub_System.Models;
using AutoHub_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
namespace AutoHub_System.Controllers
{
    [Authorize(Roles = "User")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ICarService _carService;
        private readonly UserManager<User> _userManager;
        private readonly IDepositePolicyService _policyService;
        public PaymentController(IPaymentService paymentService, ICarService carService, UserManager<User> userManager, IDepositePolicyService policyService) 
        {
            _paymentService = paymentService;
            _carService = carService;
            _userManager = userManager;
            _policyService = policyService;
        }
        
       
        [HttpPost]
        public async Task<IActionResult> Index(int carId)
        {
            var car = await _carService.GetByIdAsync(carId);
            if (car == null)
                return NotFound();

            // جلب البوليسي النشطة
            var activePolicy = await _policyService.GetActivePolicyAsync();
            var selectedPolicy = activePolicy ?? _policyService.get_all().FirstOrDefault();

            if (selectedPolicy == null)
                return BadRequest("لا توجد سياسة عربون مفعلة حالياً");

            decimal depositRate = selectedPolicy.DepositeRate; 
            decimal depositAmount = car.Price * depositRate;

            var vm = new PaymentViewModel
            {
                CarID = car.CarID,
                Deposit = depositAmount,
                DepositRate = depositRate,          
                CarBrand = car.Brand,
                CarModel = car.Model,
                Price = car.Price
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(PaymentViewModel model)
        {
            var selectedCar = await _carService.GetByIdAsync(model.CarID);

            if (!ModelState.IsValid)
            {
                if (selectedCar != null)
                {
                    // إعادة حساب النسبة تاني عشان لو اليوزر عدّل الفورم يدوي
                    var activePolicy = await _policyService.GetActivePolicyAsync();
                    var policy = activePolicy ?? _policyService.get_all().FirstOrDefault();
                    decimal rate = policy?.DepositeRate ?? 0.1m;

                    model.CarBrand = selectedCar.Brand;
                    model.CarModel = selectedCar.Model;
                    model.Price = selectedCar.Price;
                    model.Deposit = selectedCar.Price * rate;
                    model.DepositRate = rate;
                }
                return View("Index", model);
            }

            if (selectedCar == null)
            {
                ModelState.AddModelError("", "Car Not Exist");
                return View("Index", model);
            }

            
            var depositAmount = model.Deposit;
            var amountInUsd = Math.Round(depositAmount / 48.5m, 2);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = amountInUsd * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                // هنا نعرض النسبة الفعلية
                                Name = $"{(model.DepositRate * 100):F0}% Deposit - {selectedCar.Brand} {selectedCar.Model}"
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

           
            TempData["CarId"] = selectedCar.CarID;
            TempData["FullPrice"] = selectedCar.Price.ToString("F0");
            TempData["Deposit"] = depositAmount.ToString("F0");
            TempData["DepositRate"] = model.DepositRate.ToString(); 
            TempData["Name"] = model.Name;
            TempData["Email"] = model.Email;
            TempData["Phone"] = model.Phone;
            TempData["Phone2"] = model.Phone2;
            TempData["SSN"] = model.SSN;

            TempData.Keep();

            return Redirect(session.Url);
        }
       
        public async Task<IActionResult> Success(string session_id)
        {
            if (string.IsNullOrEmpty(session_id))
                return RedirectToAction("Cancel");

            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(session_id);

            if (session.PaymentStatus != "paid")
                return RedirectToAction("Cancel");

            
            if (TempData["CarId"] is not int carId || carId <= 0)
                return RedirectToAction("Cancel");

            var selectedCar = await _carService.GetByIdAsync(carId);
            if (selectedCar == null)
                return RedirectToAction("Cancel");

            var deposit = decimal.Parse(TempData["Deposit"]?.ToString() ?? "0");
            var fullPrice = decimal.Parse(TempData["FullPrice"]?.ToString() ?? "0");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Cancel");

            var payment = new PaymentInfo
            {
                Name = TempData["Name"]?.ToString() ?? "",
                Email = TempData["Email"]?.ToString() ?? "",
                Phone = TempData["Phone"]?.ToString() ?? "",
                Phone2 = TempData["Phone2"]?.ToString(),
                SSN = TempData["SSN"]?.ToString() ?? "",
                Amount = deposit,
                CreatedAt = DateTime.Now
            };

            await _paymentService.SavePaymentAsync(payment);

            var activePolicy = await _policyService.GetActivePolicyAsync();
            int policyId = activePolicy?.PolicyID ?? 0;

            if (policyId == 0)
            {
                var allPolicies = _policyService.get_all(); 
                var fallbackPolicy = allPolicies.FirstOrDefault(); 
                policyId = fallbackPolicy?.PolicyID ?? 1;
            }
            
            var order = new Order
            {
                TotalPaid = (float)deposit,
                PriceWhenBook = (float)fullPrice,
                Status = "Pending",
                OrderDate = DateTime.Now,
                CarId = carId,
                UserId = currentUser.Id,
                DepositePolicyId = policyId,           
                PaymentInfoId = payment.Id,
                PaymentInfo = payment                 
            };

            await _paymentService.CreateOrderAsync(order);

            await _paymentService.UpdatePaymentOrderIdAsync(payment.Id, order.OrderID);

            ViewBag.CustomerName = payment.Name;
            ViewBag.DepositAmount = deposit.ToString("N0");
            ViewBag.CarName = $"{selectedCar.Brand} {selectedCar.Model}";
            ViewBag.OrderId = order.OrderID;
            
            TempData.Keep();

            return View("Success");
        }

        public IActionResult Cancel()
        {
            return View();
        }

    }
}