// ICarRepository.cs
using AutoHub_System.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoHub_System.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        bool CarExists(int carId);
        Task<bool> CarExistsAsync(int carId);
        List<Car> GetByBrand(string brand);
        Task<List<Car>> GetByBrandAsync(string brand);
        List<Car> GetAvailableCars();
        Task<List<Car>> GetAvailableCarsAsync();
        List<Car> GetCarsByStatus(string status);
        Task<List<Car>> GetCarsByStatusAsync(string status);
        // Synchronous methods
        new List<Car> GetAll();
        new Car? GetById(int id);
        // Asynchronous methods
        Task<List<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(int id);
    }
}