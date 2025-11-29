using AutoHub_System.Data;
using AutoHub_System.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoHub_System.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PaymentInfo payment)
        {
            _context.PaymentInfos.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderIdAsync(int paymentId, int orderId)
        {
            await _context.PaymentInfos
                .Where(p => p.Id == paymentId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.OrderId, orderId));
        }
    }
}
