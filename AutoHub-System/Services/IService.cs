namespace AutoHub_System.Services
{
    public interface IService<T> where T : class
    {
       
        List<T> get_all();

        T? find_id(int id);
    }
}
