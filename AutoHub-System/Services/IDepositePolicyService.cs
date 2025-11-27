// Services/Interfaces/IDepositePolicyService.cs
using AutoHub_System.Models;
namespace AutoHub_System.Services
{
    public interface IDepositePolicyService : IService<DepositePolicy>
    {
        DepositePolicy GetActivePolicy();
        bool HasActivePolicy();
        List<DepositePolicy> GetPoliciesByDateRange(DateTime startDate, DateTime endDate);
        void DeactivateOtherPolicies(int currentPolicyId);
    }
}