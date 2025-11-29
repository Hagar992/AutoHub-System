namespace AutoHub_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepositePolicyController : BaseController
    {
        private readonly IDepositePolicyService _policyService;

        public DepositePolicyController(IDepositePolicyService policyService,UserManager<User> userManager) : base(userManager)
        {
            _policyService = policyService;
        }

        // GET: DepositePolicy
        public IActionResult Index()
        {
            var policies = _policyService.get_all();
            return View(policies);
        }


        // GET: DepositePolicy/Create
        public IActionResult Create()
        {
            var viewModel = new DepositePolicyViewModel
            {
                EffectiveDate = DateTime.Now,
                IsActive = !_policyService.HasActivePolicy() // Auto-activate if no active policy exists
            };

            ViewBag.HasActivePolicy = _policyService.HasActivePolicy();
            return View(viewModel);
        }

        // POST: DepositePolicy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepositePolicyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var policy = new DepositePolicy
                {
                    EffectiveDate = viewModel.EffectiveDate,
                    DepositeRate = (decimal)viewModel.DepositeRate,
                    IsActive = viewModel.IsActive
                };

                _policyService.Add(policy);
                TempData["Success"] = "Deposit policy created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.HasActivePolicy = _policyService.HasActivePolicy();
            return View(viewModel);
        }

        // GET: DepositePolicy/Edit/5
        public IActionResult Edit(int id)
        {
            var policy = _policyService.find_id(id);
            if (policy == null)
            {
                return NotFound();
            }

            var viewModel = new DepositePolicyViewModel
            {
                PolicyID = policy.PolicyID,
                EffectiveDate = policy.EffectiveDate,
                DepositeRate = (decimal)policy.DepositeRate,
                IsActive = policy.IsActive
            };

            // Check if there are other active policies
            var otherActivePolicies = _policyService.get_all()
                .Any(p => p.IsActive && p.PolicyID != id);
            ViewBag.HasOtherActivePolicy = otherActivePolicies;

            return View(viewModel);
        }

        // POST: DepositePolicy/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DepositePolicyViewModel viewModel)
        {
            if (id != viewModel.PolicyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var policy = _policyService.find_id(id);
                if (policy == null)
                {
                    return NotFound();
                }

                policy.EffectiveDate = viewModel.EffectiveDate;
                policy.DepositeRate = (decimal)viewModel.DepositeRate;
                policy.IsActive = viewModel.IsActive;

                _policyService.Update(policy);
                TempData["Success"] = "Deposit policy updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            var otherActivePolicies = _policyService.get_all()
                .Any(p => p.IsActive && p.PolicyID != id);
            ViewBag.HasOtherActivePolicy = otherActivePolicies;

            return View(viewModel);
        }

        // في DepositePolicyController - إضافة الـ Action methods الجديدة

        // GET: DepositePolicy/ActivateConfirmation/5
        public IActionResult ActivateConfirmation(int id)
        {
            var policy = _policyService.find_id(id);
            if (policy == null)
            {
                return NotFound();
            }
            return View(policy);
        }

        // GET: DepositePolicy/DeactivateConfirmation/5
        public IActionResult DeactivateConfirmation(int id)
        {
            var policy = _policyService.find_id(id);
            if (policy == null)
            {
                return NotFound();
            }
            return View(policy);
        }

        // POST: DepositePolicy/Activate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(int id)
        {
            var policy = _policyService.find_id(id);
            if (policy == null)
            {
                return NotFound();
            }

            policy.IsActive = true;
            _policyService.Update(policy);

            TempData["Success"] = $"Policy #{policy.PolicyID} has been activated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: DepositePolicy/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(int id)
        {
            var policy = _policyService.find_id(id);
            if (policy == null)
            {
                return NotFound();
            }

            policy.IsActive = false;
            _policyService.Update(policy);

            TempData["Success"] = $"Policy #{policy.PolicyID} has been deactivated successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}