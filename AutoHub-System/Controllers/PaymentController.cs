using Microsoft.AspNetCore.Mvc;
using AutoHub_System.Services;
using AutoHub_System.Models;
using Stripe.Checkout;

namespace AutoHub_System.Controllers
{
    public class PaymentController : Controller
    {
         private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(PaymentInfo model)
        {
            var amount = 100.00m;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = amount * 100,
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Service Payment"
                        }
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Home", null, Request.Scheme) + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = Url.Action("Cancel", "Home", null, Request.Scheme),
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            
            TempData["Name"] = model.Name;
            TempData["Email"] = model.Email;
            TempData["Phone"] = model.Phone;
            TempData["Phone2"] = model.Phone2;
            TempData["SSN"] = model.SSN;
            TempData["Amount"] = amount.ToString();

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

