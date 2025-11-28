namespace AutoHub_System.Models
{
    public class Car
    {
        public int CarID { get; set; }
        public List<string> CarImage { get; set; }
        public List<string> Feature { get; set; }
        public int SeatNumer { get; set; }
        public string Brand { get; set; }
        public string CarSatus { get; set; }
        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Color { get; set; }
        public string EngineType { get; set; }
        public decimal Price { get; set; }
        public string Transimission { get; set; }
        public int Fuel_Economy { get; set; }
        // Relationship
        public string MainImage => CarImage != null && CarImage.Any()
        ? CarImage.First()
        : "~/img/default-car.jpg";

        public ICollection<Order> Orders { get; set; }
    }
}