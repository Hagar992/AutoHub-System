using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AutoHub_System.Models
{
    public class Order
    {
        [Key]  
        public int OrderID { get; set; } 

        [Required]
        public float TotalPaid { get; set; } // the costomer paed => doha
        
        [Required]
        public float PriceWhenBook { get; set; }  // car price in the date coustomer booked it in => doha

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Default to Pending

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
      
        public DateTime? BuyingDate { get; set;}  

        // Foreign Keys
        [Required]
        public int DepositePolicyId{ get; set; }

        [Required]
        public int CarId{ get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int PaymentInfoId { get; set; } //DOHA

        // Navigation Properties
        [ForeignKey("UserId")]
        public User User { get; set; }


        [ForeignKey("DepositePolicyId")]
        public DepositePolicy DepositePolicy { get; set; }


        [ForeignKey("CarId")]
        public Car Car { get; set; }


        [ForeignKey("PaymentInfoId")]
        public PaymentInfo PaymentInfo { get; set; } = null!; //DOHA
    }
}