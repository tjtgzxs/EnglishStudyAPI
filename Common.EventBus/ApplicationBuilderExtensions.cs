using Microsoft.AspNetCore.Builder;

namespace Common.EventBus;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEventBus(this IApplicationBuilder appBuilder)
    {
        object? eventBus = appBuilder.ApplicationServices.GetService(typeof(IEventBus));
        if (eventBus == null)
        {
            throw new ApplicationException("No event bus registered");
        }

        return appBuilder;
    }
    
}