﻿using System.Text.Json;

namespace Common.EventBus;

public abstract class JsonIntegrationEventHandler<T>:IIntegrationEventHandler
{
    public Task Handle(string eventName, string json)
    {
        T? eventData = JsonSerializer.Deserialize<T>(json);
        return HandleJson(eventName,eventData);
    }
    public abstract Task HandleJson(string eventName, T? eventData);
}