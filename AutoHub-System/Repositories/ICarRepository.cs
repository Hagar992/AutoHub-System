// ICarRepository.cs
using AutoHub_System.Models;
using System.Collections.Generic;

namespace AutoHub_System.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        bool CarExists(int carId);
        List<Car> GetByBrand(string brand);
        List<Car> GetAvailableCars();
        List<Car> GetCarsByStatus(string status);
    }
}