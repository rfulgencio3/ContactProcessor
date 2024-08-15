using ContactProcessor.Application.Services;
using ContactProcessor.Application.Services.Interfaces;
using ContactProcessor.Core.Interfaces;
using ContactProcessor.Infrastructure.Data;
using ContactProcessor.Worker;
using ContactProcessor.Worker.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ContactDbContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactService, ContactService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateContactConsumer>();
            x.AddConsumer<UpdateContactConsumer>();
            x.AddConsumer<DeleteContactConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", host => { });

                cfg.UseRawJsonSerializer();

                cfg.ReceiveEndpoint("contact.create", e =>
                {
                    e.ConfigureConsumer<CreateContactConsumer>(context);
                });

                cfg.ReceiveEndpoint("contact.update", e =>
                {
                    e.ConfigureConsumer<UpdateContactConsumer>(context);
                });

                cfg.ReceiveEndpoint("contact.delete", e =>
                {
                    e.ConfigureConsumer<DeleteContactConsumer>(context);
                });
            });
        });

        services.AddMassTransitHostedService(true);

        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();
