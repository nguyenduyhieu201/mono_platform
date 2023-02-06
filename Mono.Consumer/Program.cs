using MassTransit;
using Mono.Consumer.Consumers;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(config => {

    config.AddConsumer<NotificationConsumer>();

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration["RabbitMQSettings:HostAddress"]);

        cfg.ReceiveEndpoint("notification-queue", c => {
            c.ConfigureConsumer<NotificationConsumer>(ctx);
            c.Bind("mono-exchange", e =>
            {
                e.RoutingKey = "notification";
                e.ExchangeType = ExchangeType.Direct;
            });
        });
    });
});

var app = builder.Build();

app.Run();