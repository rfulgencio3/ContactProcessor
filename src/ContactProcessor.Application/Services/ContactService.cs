using ContactProcessor.Core.Entities;
using ContactProcessor.Core.Interfaces;

namespace ContactProcessor.Application.Services;

public class ContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<int> HandleCreateContactAsync(Contact contact)
    {
        await _contactRepository.CreateContactAsync(contact);
        return contact.Id; // Retorna o ID do contato criado
    }

    public async Task HandleUpdateContactAsync(Contact contact)
    {
        await _contactRepository.UpdateContactAsync(contact);
    }

    public async Task HandleDeleteContactAsync(int id)
    {
        await _contactRepository.DeleteContactAsync(id);
    }
}
