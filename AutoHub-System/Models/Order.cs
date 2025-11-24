using System;

namespace AutoHub_System.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public float TotalPrice { get; set; }
        public float PricWhenBook { get; set; }
        public string Staetus { get; set; }
        public DateTime OrderDate { get; set; }

        // Foreign Keys
        public int UserID { get; set; }
        public int PolicyID { get; set; }
        public int CarID { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public DepositePolicy DepositePolicy { get; set; }
        public Car Car { get; set; }
    }
}
