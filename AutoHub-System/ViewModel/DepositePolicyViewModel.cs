
using System;
using System.ComponentModel.DataAnnotations;

namespace AutoHub_System.ViewModel
{
    public class DepositePolicyViewModel
    {
        public int PolicyID { get; set; }

        [Required(ErrorMessage = "Effective date is required")]
        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Deposit rate is required")]
        [Range(0.01, 1.0, ErrorMessage = "Deposit rate must be between 0.01 and 1.0")]
        [Display(Name = "Deposit Rate")]
        public decimal DepositeRate { get; set; } // Default 10%

        [Display(Name = "Active Policy")]
        public bool IsActive { get; set; } = true;
    }
}