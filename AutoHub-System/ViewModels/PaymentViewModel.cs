using System.ComponentModel.DataAnnotations;

namespace AutoHub_System.ViewModels
{
    public class PaymentViewModel
    {
        public int CarID { get; set; }
        public string? CarBrand { get; set; }
        public string? CarModel { get; set; }
        public decimal Price { get; set; }
        public decimal Deposit { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format should be like yourName12@gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number")]
        public string Phone { get; set; } = string.Empty;

        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number")]
        public string? Phone2 { get; set; }

        [Required(ErrorMessage = "National ID is required")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits")]
        public string SSN { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
