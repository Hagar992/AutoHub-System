// Repositories/DepositePolicyRepository.cs
using AutoHub_System.Data;
using AutoHub_System.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoHub_System.Repositories
{
    public class DepositePolicyRepository : Repository<DepositePolicy>, IDepositePolicyRepository
    {
        private readonly ApplicationDbContext _context;

        public DepositePolicyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public DepositePolicy GetActivePolicy()
        {
            return _context.DepositePolicies
                .FirstOrDefault(p => p.IsActive && p.EffectiveDate <= DateTime.Now);
        }

        public bool HasActivePolicy()
        {
            return _context.DepositePolicies.Any(p => p.IsActive && p.EffectiveDate <= DateTime.Now);
        }

        public List<DepositePolicy> GetPoliciesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.DepositePolicies
                .Where(p => p.EffectiveDate >= startDate && p.EffectiveDate <= endDate)
                .OrderByDescending(p => p.EffectiveDate)
                .ToList();
        }
        public async Task<DepositePolicy?> GetActivePolicyAsync() //DOHA
        {
            return await _context.DepositePolicies
                .FirstOrDefaultAsync(p => p.IsActive);
        }
    }
}