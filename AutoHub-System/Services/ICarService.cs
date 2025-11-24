using AutoHub_System.Models;

namespace AutoHub_System.Services
{
    public interface ICarService
    {
        Task<List<Car>> GetRandomMostPickedCarsAsync(int count = 3);
        Task<Car?> GetByIdAsync(int id);
    }
}
