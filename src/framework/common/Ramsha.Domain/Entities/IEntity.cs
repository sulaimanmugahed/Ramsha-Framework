using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Domain;

public interface IHasId
{
    object GetId();
}

public interface IHasId<TId> : IHasId
where TId : IEquatable<TId>
{
    TId Id { get; }
}

public interface IEntity
{

}

public interface IEntity<TId> : IEntity, IHasId<TId>
where TId : IEquatable<TId>

{
}

public class Entity : IEntity
{

}

public abstract class Entity<TId> : IEntity<TId>
where TId : IEquatable<TId>

{
    public TId Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public object GetId() => Id;

}
