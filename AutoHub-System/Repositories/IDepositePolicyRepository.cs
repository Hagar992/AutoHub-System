
using AutoHub_System.Models;


namespace AutoHub_System.Repositories
{
    public interface IDepositePolicyRepository : IRepository<DepositePolicy>
    {
        DepositePolicy GetActivePolicy();
        bool HasActivePolicy();
        List<DepositePolicy> GetPoliciesByDateRange(DateTime startDate, DateTime endDate);
        Task<DepositePolicy?> GetActivePolicyAsync();
    }
}