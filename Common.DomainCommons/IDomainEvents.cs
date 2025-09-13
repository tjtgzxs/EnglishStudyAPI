using MediatR;

namespace Common.DomainCommons;

public interface IDomainEvents
{
    IEnumerable<INotification> GetDomainEvents();
    void AddDomainEvent(INotification eventItem);
    void AddDomainEventIfAbsent(INotification eventItem);
    void ClearDomainEvents();
}