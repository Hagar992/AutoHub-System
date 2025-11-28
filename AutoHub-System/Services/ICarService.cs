// ICarService.cs
using AutoHub_System.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoHub_System.Services
{
    public interface ICarService : IService<Car>
    {
        bool CarExists(int carId);
        List<Car> GetByBrand(string brand);
        List<Car> GetAvailableCars();
        List<Car> GetCarsByStatus(string status);
        List<Car> SearchCars(string searchTerm);
        List<Car> GetCarsByPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Car>> GetRandomMostPickedCarsAsync(int count = 3);
        Task<Car?> GetByIdAsync(int id);
    }
}