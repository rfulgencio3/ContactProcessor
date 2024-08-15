using ContactProcessor.Application.Models;
using ContactProcessor.Application.Services;
using ContactProcessor.Application.IntegrationModels;
using ContactProcessor.Core.Entities;
using MassTransit;

namespace ContactProcessor.Worker.Consumers;

public class UpdateContactConsumer : IConsumer<UpdateContactModel>
{
    private readonly ContactService _contactService;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateContactConsumer(ContactService contactService, IPublishEndpoint publishEndpoint)
    {
        _contactService = contactService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<UpdateContactModel> context)
    {
        var model = context.Message;

        var contact = new Contact
        {
            Id = model.Id,
            DDD = model.DDD,
            Number = model.Number,
            FullName = model.FullName,
            Status = model.Status
        };

        await _contactService.HandleUpdateContactAsync(contact);

        var integrationMessage = new ContactIntegrationModel
        {
            Id = model.Id,
            DDD = model.DDD,
            Number = model.Number,
            FullName = model.FullName,
            Status = model.Status,
            OperationType = "update"
        };

        await _publishEndpoint.Publish(integrationMessage);
    }
}
