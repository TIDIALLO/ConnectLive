namespace ConnectLive.Application;
public class IntegrationEvent : IIntegrationEvent
{
    public IntegrationEvent()
    {
        EventId = Guid.NewGuid();
    }

    public IntegrationEvent(Guid eventId)
    {
        EventId = eventId;
    }

    public Guid EventId { get; set; } = Guid.NewGuid();
}

public interface IIntegrationEvent
{
    public Guid EventId { get; set; }
}
