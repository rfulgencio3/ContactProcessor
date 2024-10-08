﻿using ContactProcessor.Application.Event;
using ContactProcessor.Application.IntegrationModels;
using ContactProcessor.Application.Services.Interfaces;
using MassTransit;

namespace ContactProcessor.Worker.Consumers;

public class DeleteContactConsumer : IConsumer<DeleteContactEvent>
{
    private readonly IContactService _contactService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DeleteContactConsumer> _logger;

    public DeleteContactConsumer(IContactService contactService, IPublishEndpoint publishEndpoint, ILogger<DeleteContactConsumer> logger)
    {
        _contactService = contactService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteContactEvent> context)
    {
        _logger.LogInformation("Received DeleteContact message at: {time}", DateTimeOffset.Now);

        var model = context.Message;

        await _contactService.DeleteContactHandlerAsync(model.Id);

        var integrationMessage = new DeleteIntegrationModel
        {
            Id = model.Id,
            OperationType = "delete"
        };

        await _publishEndpoint.Publish(integrationMessage);
        _logger.LogInformation("Published integration message for DeleteContact at: {time}", DateTimeOffset.Now);
    }
}
