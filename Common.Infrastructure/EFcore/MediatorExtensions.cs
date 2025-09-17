using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zack.DomainCommons.Models;

namespace Common.Infrastructure.EFcore;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return services.AddMediatR(assemblies.ToArray());
    }

    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext dbContext)
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<IDomainEvents>()
            .Where(x => x.Entity.GetDomainEvents().Any());
        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.GetDomainEvents())
            .ToList();//加ToList()是为立即加载，否则会延迟执行，到foreach的时候已经被ClearDomainEvents()了

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
    
}