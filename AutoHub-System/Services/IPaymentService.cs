using AutoHub_System.Models;

namespace AutoHub_System.Services
{
    public interface IPaymentService
    {
        Task SavePaymentAsync(PaymentInfo payment);
    }
}
