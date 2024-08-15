using ContactProcessor.Application.Models;
using ContactProcessor.Application.Services;
using ContactProcessor.Application.IntegrationModels;
using ContactProcessor.Core.Entities;
using MassTransit;

namespace ContactProcessor.Worker.Consumers;

public class CreateContactConsumer : IConsumer<CreateContactModel>
{
    private readonly ContactService _contactService;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateContactConsumer(ContactService contactService, IPublishEndpoint publishEndpoint)
    {
        _contactService = contactService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<CreateContactModel> context)
    {
        var model = context.Message;

        var contact = new Contact
        {
            DDD = model.DDD,
            Number = model.Number,
            FullName = model.FullName,
            Status = model.Status
        };

        int id = await _contactService.HandleCreateContactAsync(contact);

        var integrationMessage = new ContactIntegrationModel
        {
            Id = id,
            DDD = model.DDD,
            Number = model.Number,
            FullName = model.FullName,
            Status = model.Status,
            OperationType = "create"
        };

        await _publishEndpoint.Publish(integrationMessage);
    }
}
