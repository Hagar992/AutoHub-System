using AutoHub_System.Models;

namespace AutoHub_System.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(PaymentInfo payment);
        Task UpdateOrderIdAsync(int paymentId, int orderId);
    }
}
