﻿namespace Common.EventBus;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EventNameAttribute:Attribute
{
    public string Name { get; init; }
    public EventNameAttribute(string eventName)
    {
        Name=eventName;
    }
}