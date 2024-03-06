using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public record BaseEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTimeOffset Creat { get; init; } = DateTimeOffset.UtcNow;

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
