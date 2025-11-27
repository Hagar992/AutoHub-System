// CarRepository.cs
using AutoHub_System.Data;
using AutoHub_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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

        // Removed LicensePlateExists method

        public List<Car> GetByBrand(string brand)
        {
            return _context.Cars
                .Where(c => c.Brand.ToLower() == brand.ToLower())
                .ToList();
        }

        public List<Car> GetAvailableCars()
        {
            return _context.Cars
                .Where(c => c.CarSatus.ToLower() == "available")
                .ToList();
        }

        public List<Car> GetCarsByStatus(string status)
        {
            return _context.Cars
                .Where(c => c.CarSatus.ToLower() == status.ToLower())
                .ToList();
        }

        // Override GetAll to include related entities if needed
        public new List<Car> GetAll()
        {
            return _context.Cars
                .Include(c => c.Orders)
                .ToList();
        }

        // Override GetById to include related entities if needed
        public new Car? GetById(int id)
        {
            return _context.Cars
                .Include(c => c.Orders)
                .FirstOrDefault(c => c.CarID == id);
        }
    }
}