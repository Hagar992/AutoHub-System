using AutoHub_System.Data;
using AutoHub_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoHub_System.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool CarExists(int carId)
        {
            return _context.Cars.Any(c => c.CarID == carId);
        }

        public async Task<bool> CarExistsAsync(int carId)
        {
            return await _context.Cars.AnyAsync(c => c.CarID == carId);
        }

        public List<Car> GetByBrand(string brand)
        {
            return _context.Cars
                .Where(c => c.Brand.ToLower() == brand.ToLower())
                .ToList();
        }

        public async Task<List<Car>> GetByBrandAsync(string brand)
        {
            return await _context.Cars
                .Where(c => c.Brand.ToLower() == brand.ToLower())
                .ToListAsync();
        }

        public List<Car> GetAvailableCars()
        {
            return _context.Cars
                .Where(c => c.CarSatus.ToLower() == "available")
                .ToList();
        }

        public async Task<List<Car>> GetAvailableCarsAsync()
        {
            return await _context.Cars
                .Where(c => c.CarSatus.ToLower() == "available")
                .ToListAsync();
        }

        public List<Car> GetCarsByStatus(string status)
        {
            return _context.Cars
                .Where(c => c.CarSatus.ToLower() == status.ToLower())
                .ToList();
        }

        public async Task<List<Car>> GetCarsByStatusAsync(string status)
        {
            return await _context.Cars
                .Where(c => c.CarSatus.ToLower() == status.ToLower())
                .ToListAsync();
        }

        // Basic methods - synchronous
        public new List<Car> GetAll()
        {
            return _context.Cars
                .Include(c => c.Orders)
                .ToList();
        }

        public new Car? GetById(int id)
        {
            return _context.Cars
                .Include(c => c.Orders)
                .FirstOrDefault(c => c.CarID == id);
        }

        // Basic methods - asynchronous (same names with Async)
        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars
                .Include(c => c.Orders)
                .ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.CarID == id);
        }
    }
}