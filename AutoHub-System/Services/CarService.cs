using AutoHub_System.Models;
using AutoHub_System.Repositories;

namespace AutoHub_System.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly List<Car> _dummyCars = new()
    {
        new Car { CarID = 1, Brand = "BMW", Model = "3 Series", CarImage = new() { "~/img/default-car.jpg", }, Description = "2.0L — 5 seats", Price = 65 },
        new Car { CarID = 2, Brand = "Mercedes", Model = "E-Class", CarImage = new() { "~/img/default-car.jpg" }, Description = "Luxury", Price = 75 },
        new Car { CarID = 3, Brand = "Tesla", Model = "Model 3", CarImage = new() { "~/img/default-car.jpg" }, Description = "Electric", Price = 90 },
        };

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<List<Car>> GetRandomMostPickedCarsAsync(int count = 3)
        {
            var cars = await _carRepository.GetAllCarsAsync();

            if (!cars.Any())
                cars = _dummyCars;

            var random = new Random();
            return cars.OrderBy(x => random.Next()).Take(count).ToList();
        }
    }
}
