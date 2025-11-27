namespace AutoHub_System.Models
{
    public class DepositePolicy
    {

        public int PolicyID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsActive { get; set; }
        public float DepositeRate { get; set; }

        // Relationship
        public ICollection<Order> Orders { get; set; }
    }
}