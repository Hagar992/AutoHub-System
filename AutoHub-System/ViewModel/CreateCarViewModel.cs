
using System.ComponentModel.DataAnnotations;

namespace AutoHub_System.ViewModel
{
    public class CreateCarViewModel
    {
        [Required(ErrorMessage = "Brand is required")]
        [StringLength(50, ErrorMessage = "Brand cannot exceed 50 characters")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2025, ErrorMessage = "Year must be between 1900 and 2025")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [StringLength(30, ErrorMessage = "Color cannot exceed 30 characters")]
        public string Color { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Mileage must be a positive value")]
        public int Mileage { get; set; }

        [StringLength(50, ErrorMessage = "Engine type cannot exceed 50 characters")]
        public string EngineType { get; set; }

        [StringLength(30, ErrorMessage = "Transmission cannot exceed 30 characters")]
        public string Transmission { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Fuel economy must be a positive value")]
        public int FuelEconomy { get; set; }

        [Range(1, 10, ErrorMessage = "Seat number must be between 1 and 10")]
        public int SeatNumber { get; set; } = 5;

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity{ get; set; } = 1;

        public string CarStatus { get; set; } = "Available";

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public List<string> Features { get; set; } = new List<string>();

        // For image uploads
        public IFormFileCollection Images { get; set; }
        public IFormFile MainImage { get; set; }
    }
}