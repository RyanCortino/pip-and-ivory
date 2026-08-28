namespace PipAndIvory.Domain.Common.Interfaces;

public interface IHasDomainEvents
{
    IReadOnlyCollection<BaseEvent> DomainEvents { get; }

    void AddDomainEvent(BaseEvent domainEvent);

    void ClearDomainEvents();

    void RemoveDomainEvent(BaseEvent domainEvent);
}
