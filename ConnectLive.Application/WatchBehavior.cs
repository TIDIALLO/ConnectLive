using ConnectLive.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConnectLive.Application;

public class WatchBehavior<TRequest, TResponse> 
    (ILogger<WatchBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<WatchBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var integrationEvent = request as IIntegrationEvent;
        Guid eventId = Guid.NewGuid();
        if (integrationEvent != null)
        {
            eventId = integrationEvent.EventId;
        }

        _logger.LogInformation(">>>>>>>[{commandName}][{EventID}][START]", typeof(TRequest).Name, eventId);
        Stopwatch stopWatcher = Stopwatch.StartNew();
        
        var result = await next();
        
        stopWatcher.Stop();
        long elapsed = stopWatcher.ElapsedMilliseconds;
        _logger.LogInformation($">>>>>>>[{{commandName}}][{{EventID}}][END]: Executed in {elapsed} ms", typeof(TRequest).Name, eventId);

        return result;
    }
}
