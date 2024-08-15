using ContactProcessor.Application.Services;
using ContactProcessor.Core.Interfaces;
using ContactProcessor.Infrastructure.Data;
using ContactProcessor.Worker.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ContactDbContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ContactService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateContactConsumer>();
            x.AddConsumer<UpdateContactConsumer>();
            x.AddConsumer<DeleteContactConsumer>();

            x.UsingRabbitMq((context, config) =>
            {
                config.Host("localhost", "/", h => { });

                config.ReceiveEndpoint("contact.create", e =>
                {
                    e.ConfigureConsumer<CreateContactConsumer>(context);
                });

                config.ReceiveEndpoint("contact.update", e =>
                {
                    e.ConfigureConsumer<UpdateContactConsumer>(context);
                });

                config.ReceiveEndpoint("contact.delete", e =>
                {
                    e.ConfigureConsumer<DeleteContactConsumer>(context);
                });
            });
        });

        services.AddMassTransitHostedService();
    })
    .Build();

await host.RunAsync();
