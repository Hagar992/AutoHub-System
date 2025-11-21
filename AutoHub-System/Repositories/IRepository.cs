namespace AutoHub_System.Repositories
{
    public interface IRepository<T> where T : class
    {
        public List<T> get_all();

        public T? find_id(int id);
    }
}
