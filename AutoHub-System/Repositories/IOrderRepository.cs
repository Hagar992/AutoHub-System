namespace AutoHub_System.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
