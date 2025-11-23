using AutoHub_System.Models;

namespace AutoHub_System.Repositories
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
    }
}
