using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Ramsha.Common.Domain;

public interface IHasDomainEvent
{
    IReadOnlyList<DomainEventRecord> GetEvents();
    void ClearEvents();
}


public class DomainEventRecord
{
    public DomainEventRecord(object data)
    {
        Data = data;
    }
    public object Data { get; init; } = default!;
}

public interface IAggregateRoot<TId> : IEntity<TId>, IHasDomainEvent
where TId : IEquatable<TId>

{

}


public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
where TId : IEquatable<TId>
{
    private readonly List<DomainEventRecord> _events = [];

    public IReadOnlyList<DomainEventRecord> GetEvents()
    {
        return _events;
    }

    public void RaiseEvent<T>(T eventData)
    where T : notnull
    {
        DomainEventRecord eventRecord = new(eventData);
        _events.Add(eventRecord);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    public AggregateRoot()
    {

    }
    public AggregateRoot(TId id)
    {
        Id = id;
    }
}
