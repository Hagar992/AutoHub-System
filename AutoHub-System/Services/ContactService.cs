using AutoHub_System.Models;
using AutoHub_System.Repositories;

namespace AutoHub_System.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task SendMessageAsync(string name, string email, string message)
        {
            var contactMessage = new ContactMessage
            {
                Name = name,
                Email = email,
                Message = message
            };

            await _contactRepository.AddMessageAsync(contactMessage);
        }
    }
}
