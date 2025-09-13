using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace Common.DomainCommons;

public record BaseEntity : IEntity,IDomainEvents
{
    [NotMapped]
    private List<INotification> _domainEvents = new List<INotification>();
    public Guid Id { get; protected set; }
    public IEnumerable<INotification> GetDomainEvents()
    {
        return _domainEvents;
    }

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void AddDomainEventIfAbsent(INotification eventItem)
    {
        if (!_domainEvents.Contains(eventItem))
        {
            _domainEvents.Add(eventItem);
        }
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}