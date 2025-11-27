
using System.ComponentModel.DataAnnotations;
namespace AutoHub_System.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        [Required]
        public float PriceWhenBook { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Default to Pending

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Foreign Keys
        [Required]
        public int PolicyID { get; set; }

        [Required]
        public int CarID { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public DepositePolicy DepositePolicy { get; set; }
        public Car Car { get; set; }
    }
}