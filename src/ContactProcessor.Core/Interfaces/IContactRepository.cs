using ContactProcessor.Core.Entities;

namespace ContactProcessor.Core.Interfaces
{
    public interface IContactRepository
    {
        Task CreateContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
    }
}
