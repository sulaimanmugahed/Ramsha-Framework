using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public interface IObjectAccessor<out T>
{
    T? Value { get; }
}

public class ObjectAccessor<T> : IObjectAccessor<T>
{
    private readonly object _syncLock = new();
    private T? _value;

    public T? Value
    {
        get
        {
            lock (_syncLock)
            {
                return _value;
            }
        }
        set
        {
            lock (_syncLock)
            {
                _value = value;
            }
        }
    }

    public ObjectAccessor()
    {

    }

    public ObjectAccessor(T? obj)
    {
        Value = obj;
    }
}