﻿using ContactProcessor.Application.Events;
using ContactProcessor.Application.IntegrationModels;
using ContactProcessor.Application.Services.Interfaces;
using ContactProcessor.Core.Entities;
using MassTransit;

namespace ContactProcessor.Worker.Consumers;

public class CreateContactConsumer : IConsumer<CreateContactEvent>
{
    private readonly IContactService _contactService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateContactConsumer> _logger;

    public CreateContactConsumer(IContactService contactService, IPublishEndpoint publishEndpoint, ILogger<CreateContactConsumer> logger)
    {
        _contactService = contactService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateContactEvent> context)
    {
        _logger.LogInformation("Received CreateContact message at: {time}", DateTimeOffset.Now);

        var model = context.Message;

        var contact = new Contact
        {
            DDD = model.DDD,
            Number = model.Number,
            FullName = model.FullName,
            Status = model.Status
        };

        int id = await _contactService.CreateContactHandlerAsync(contact);

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
        _logger.LogInformation("Published integration message for CreateContact at: {time}", DateTimeOffset.Now);
    }
}
