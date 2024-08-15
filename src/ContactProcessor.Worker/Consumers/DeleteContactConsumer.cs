using ContactProcessor.Application.Models;
using ContactProcessor.Application.Services;
using ContactProcessor.Application.IntegrationModels;
using MassTransit;

namespace ContactProcessor.Worker.Consumers;

public class DeleteContactConsumer : IConsumer<DeleteContactModel>
{
    private readonly ContactService _contactService;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteContactConsumer(ContactService contactService, IPublishEndpoint publishEndpoint)
    {
        _contactService = contactService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<DeleteContactModel> context)
    {
        var model = context.Message;

        await _contactService.HandleDeleteContactAsync(model.Id);

        var integrationMessage = new DeleteIntegrationModel
        {
            Id = model.Id,
            OperationType = "delete"
        };

        await _publishEndpoint.Publish(integrationMessage);
    }
}
