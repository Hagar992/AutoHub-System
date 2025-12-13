using AutoHub_System.Models;

namespace AutoHub_System.Services
{
    public interface IPaymentService
    {
        Task SavePaymentAsync(PaymentInfo payment);
        Task CreateOrderAsync(Order order); //DOHA
        Task UpdateOrderAsync(Order order);
        Task UpdatePaymentOrderIdAsync(int paymentId, int orderId);
    }
}
