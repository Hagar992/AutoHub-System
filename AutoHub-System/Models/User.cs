using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AutoHub_System.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address can't exceed 200 characters")]
        public string Address { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public string ProfilePicture { get; set; } = "default.png";
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
