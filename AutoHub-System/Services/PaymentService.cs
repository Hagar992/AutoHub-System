using AutoHub_System.Models;
using AutoHub_System.Repositories;

namespace AutoHub_System.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task SavePaymentAsync(PaymentInfo payment)
        {
            // هنا ممكن نضيف أي logic زي التحقق من SSN أو إرسال إيميل مستقبلاً
            await _paymentRepository.AddAsync(payment);
        }
    }
}
