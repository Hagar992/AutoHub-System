using System.ComponentModel.DataAnnotations;

namespace AutoHub_System.Models
{
    public class StripeSettings
    {
        [Required(ErrorMessage = "SecretKey is required")]
        public string SecretKey { get; set; }

        [Required(ErrorMessage = "PublishableKey is required")]
        public string PublishableKey { get; set; }
    }
}
