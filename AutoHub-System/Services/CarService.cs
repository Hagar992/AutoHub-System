// CarService.cs
using AutoHub_System.Models;
using AutoHub_System.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AutoHub_System.Services
{
    public class CarService : GenericService<Car>, ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository) : base(carRepository)
        {
            _carRepository = carRepository;
        }

        public bool CarExists(int carId)
        {
            return _carRepository.CarExists(carId);
        }

        // LicensePlateExists method removed

        public List<Car> GetByBrand(string brand)
        {
            return _carRepository.GetByBrand(brand);
        }

        public List<Car> GetAvailableCars()
        {
            return _carRepository.GetAvailableCars();
        }

        public List<Car> GetCarsByStatus(string status)
        {
            return _carRepository.GetCarsByStatus(status);
        }

        public List<Car> SearchCars(string searchTerm)
        {
            var allCars = _carRepository.get_all();
            return allCars.Where(c =>
                c.Brand.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Model.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Color.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public List<Car> GetCarsByPriceRange(float minPrice, float maxPrice)
        {
            var allCars = _carRepository.get_all();
            return allCars.Where(c => c.Price >= minPrice && c.Price <= maxPrice).ToList();
        }

        // Additional business logic methods
        public List<Car> GetNewArrivals(int days = 7)
        {
            var allCars = _carRepository.get_all();
            var cutoffDate = System.DateTime.Now.AddDays(-days);
            return allCars.Where(c => c.DateAdded >= cutoffDate).ToList();
        }

        public List<Car> GetCarsByYearRange(int startYear, int endYear)
        {
            var allCars = _carRepository.get_all();
            return allCars.Where(c => c.Year >= startYear && c.Year <= endYear).ToList();
        }
    }
}