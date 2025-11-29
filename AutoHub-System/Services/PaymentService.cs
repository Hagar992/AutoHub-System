using AutoHub_System.Models;
using AutoHub_System.Repositories;
using Microsoft.EntityFrameworkCore;
namespace AutoHub_System.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentInfoRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

       

        public PaymentService(IPaymentRepository paymentInfoRepository, IPaymentRepository paymentRepository, IOrderRepository orderRepository )    
        {
            _paymentInfoRepository = paymentInfoRepository;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
         
        }

        public async Task SavePaymentAsync(PaymentInfo payment)
        {
            // هنا ممكن نضيف أي logic زي التحقق من SSN أو إرسال إيميل مستقبلاً
            await _paymentRepository.AddAsync(payment);

        }

        public async Task CreateOrderAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task UpdatePaymentOrderIdAsync(int paymentId, int orderId)
        {
            await _paymentInfoRepository.UpdateOrderIdAsync(paymentId, orderId);
        }
    }
}
