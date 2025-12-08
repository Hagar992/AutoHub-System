using AutoHub_System.Models;
using AutoHub_System.Repositories;
using AutoHub_System.Services.Interfaces;


namespace AutoHub_System.Services
{
    public class DepositePolicyService : IDepositePolicyService
    {
        private readonly IDepositePolicyRepository _policyRepository;

        public DepositePolicyService(IDepositePolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public List<DepositePolicy> get_all()
        {
            return _policyRepository.get_all().OrderByDescending(p => p.EffectiveDate).ToList();
        }

        public DepositePolicy find_id(int id)
        {
            return _policyRepository.find_id(id);
        }

        public void Add(DepositePolicy entity)
        {
            // If this is an active policy, deactivate others
            if (entity.IsActive)
            {
                DeactivateOtherPolicies(entity.PolicyID);
            }

            _policyRepository.Add(entity);
            _policyRepository.Save();
        }

        public void Update(DepositePolicy entity)
        {
            // If this is being activated, deactivate others
            if (entity.IsActive)
            {
                DeactivateOtherPolicies(entity.PolicyID);
            }

            _policyRepository.Update(entity);
            _policyRepository.Save();
        }

        public void Delete(DepositePolicy entity)
        {
            _policyRepository.Delete(entity);
            _policyRepository.Save();
        }

        public DepositePolicy GetActivePolicy()
        {
            return _policyRepository.GetActivePolicy();
        }

        public bool HasActivePolicy()
        {
            return _policyRepository.HasActivePolicy();
        }

        public List<DepositePolicy> GetPoliciesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _policyRepository.GetPoliciesByDateRange(startDate, endDate);
        }

        public void DeactivateOtherPolicies(int currentPolicyId)
        {
            var activePolicies = _policyRepository.get_all()
                .Where(p => p.IsActive && p.PolicyID != currentPolicyId)
                .ToList();

            foreach (var policy in activePolicies)
            {
                policy.IsActive = false;
                _policyRepository.Update(policy);
            }

            if (activePolicies.Any())
            {
                _policyRepository.Save();
            }
        }

        public async Task<DepositePolicy?> GetActivePolicyAsync()
        {
            return await _policyRepository.GetActivePolicyAsync();
        }
    }
}