using ConnectLive.Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConnectLive.Application.BusEvents;
public class UserCreatedConsumer(ILogger<UserCreatedConsumer> logger) : IConsumer<UserCreatedContractQuery>
{
    private ILogger<UserCreatedConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<UserCreatedContractQuery> context)
    {
        await Task.CompletedTask;
        _logger.LogInformation($"->->->-> [RabbitMQ] Welcome ({context.Message.FullName}) - ({context.Message.Email})");

        //TODO: invoke other command / queries and your logic.
    }
}
