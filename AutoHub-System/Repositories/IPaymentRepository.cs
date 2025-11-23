using AutoHub_System.Models;

namespace AutoHub_System.Repositories
{
    public interface IPaymentRepository
    {
        Task AddAsync(PaymentInfo payment);
    }
}
