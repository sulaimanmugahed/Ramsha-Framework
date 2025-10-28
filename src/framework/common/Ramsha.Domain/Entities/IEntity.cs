using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Domain;

public interface IEntity
{

}

public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}

public class Entity : IEntity
{

}

public abstract class Entity<TId> : IEntity<TId>
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }
}
