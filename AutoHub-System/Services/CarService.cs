
using AutoHub_System.Repositories;


namespace AutoHub_System.Services
{
    public class CarService : GenericService<Car>, ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly List<Car> _dummyCars = new()
        {
            new Car { CarID = 1, Brand = "BMW", Model = "3 Series", CarImage = new() { "~/img/default-car.jpg" }, Description = "2.0L — 5 seats", Price = 65 },
            new Car { CarID = 2, Brand = "Mercedes", Model = "E-Class", CarImage = new() { "~/img/default-car.jpg" }, Description = "Luxury", Price = 75 },
            new Car { CarID = 3, Brand = "Tesla", Model = "Model 3", CarImage = new() { "~/img/default-car.jpg" }, Description = "Electric", Price = 90 },
        };

        public CarService(ICarRepository carRepository) : base(carRepository)
        {
            _carRepository = carRepository;
        }

        // Synchronous methods
        public bool CarExists(int carId)
        {
            return _carRepository.CarExists(carId);
        }

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
            var allCars = _carRepository.GetAll();
            return allCars.Where(c =>
                c.Brand.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Model.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Color.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public List<Car> GetCarsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var allCars = _carRepository.GetAll();
            return allCars.Where(c => c.Price >= minPrice && c.Price <= maxPrice).ToList();
        }

        public List<Car> GetNewArrivals(int days = 7)
        {
            var allCars = _carRepository.GetAll();
            var cutoffDate = System.DateTime.Now.AddDays(-days);
            return allCars.Where(c => c.DateAdded >= cutoffDate).ToList();
        }

        public List<Car> GetCarsByYearRange(int startYear, int endYear)
        {
            var allCars = _carRepository.GetAll();
            return allCars.Where(c => c.Year >= startYear && c.Year <= endYear).ToList();
        }

        // Asynchronous methods
        public async Task<bool> CarExistsAsync(int carId)
        {
            return await _carRepository.CarExistsAsync(carId);
        }

        public async Task<List<Car>> GetByBrandAsync(string brand)
        {
            return await _carRepository.GetByBrandAsync(brand);
        }

        public async Task<List<Car>> GetAvailableCarsAsync()
        {
            return await _carRepository.GetAvailableCarsAsync();
        }

        public async Task<List<Car>> GetCarsByStatusAsync(string status)
        {
            return await _carRepository.GetCarsByStatusAsync(status);
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _carRepository.GetByIdAsync(id);
        }

        public async Task<List<Car>> GetRandomMostPickedCarsAsync(int count = 3)
        {
            var cars = await _carRepository.GetAllAsync();

            if (!cars.Any())
                cars = _dummyCars;

            var random = new Random();
            return cars.OrderBy(x => random.Next()).Take(count).ToList();
        }

        // Async versions of search and filter methods
        public async Task<List<Car>> SearchCarsAsync(string searchTerm)
        {
            var allCars = await _carRepository.GetAllAsync();
            return allCars.Where(c =>
                c.Brand.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Model.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Color.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public async Task<List<Car>> GetCarsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var allCars = await _carRepository.GetAllAsync();
            return allCars.Where(c => c.Price >= minPrice && c.Price <= maxPrice).ToList();
        }

        public async Task<List<Car>> GetNewArrivalsAsync(int days = 7)
        {
            var allCars = await _carRepository.GetAllAsync();
            var cutoffDate = System.DateTime.Now.AddDays(-days);
            return allCars.Where(c => c.DateAdded >= cutoffDate).ToList();
        }

        public async Task<List<Car>> GetCarsByYearRangeAsync(int startYear, int endYear)
        {
            var allCars = await _carRepository.GetAllAsync();
            return allCars.Where(c => c.Year >= startYear && c.Year <= endYear).ToList();
        }
    }
}