namespace AutoHub_System.Services
{
    public interface IContactService
    {
        Task SendMessageAsync(string name, string email, string message);
    }

}
