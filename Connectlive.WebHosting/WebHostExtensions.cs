using ConnectLive.Application.BusEvents;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConnectLive.Core.Api.Extensions;

public static class HostExtensions
{
    public static IServiceCollection AddBusRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserCreatedConsumer>();

            x.AddBus(provider => Configure(configuration, (cfg) =>
            {
                cfg.ReceiveEndpoint("users.service",
                    ep =>
                    {
                        ep.ConfigureConsumer<UserCreatedConsumer>(provider);
                    });
            }));
        });

        return services;
    }

    public static IBusControl Configure(IConfiguration configuration, Action<IRabbitMqBusFactoryConfigurator> registrationAction = null)
    {
        var connection = configuration["EventBusConnection"];
        return Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(connection, h =>
            {
                h.Username(configuration["EventBusUserName"]);
                h.Password(configuration["EventBusPassword"]);
            });
            registrationAction?.Invoke(cfg);
        });
    }
}
