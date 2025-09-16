using Dynamic.Json;

namespace Common.EventBus;

public abstract class DynamicIntegrationEventHandler:IIntegrationEventHandler
{
    public Task Handle(string eventName, string eventData)
    {
        dynamic dynamicEventData =DJson.Parse(eventData);
        return HandleDynamic(eventName,dynamicEventData);
    }
    public abstract Task HandleDynamic(string eventName, dynamic eventData);
}