using ContactProcessor.Core.Entities;

namespace ContactProcessor.Application.Services.Interfaces;

public interface IContactService
{
    Task<int> CreateContactHandlerAsync(Contact contact);
    Task UpdateContactHandlerAsync(Contact contact);
    Task DeleteContactHandlerAsync(int id);
}
