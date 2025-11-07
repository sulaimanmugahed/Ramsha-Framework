using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public sealed class OnDispose : IDisposable
{
    private Action? _onDispose;
    public OnDispose(Action onDispose) => _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
    public void Dispose()
    {
        _onDispose?.Invoke();
        _onDispose = null;
    }
}

public class OnDispose<T> : IDisposable
{
    private readonly Action<T> _action;

    private readonly T? _parameter;

    public OnDispose(Action<T> action, T parameter)
    {
        _action = action;
        _parameter = parameter;
    }

    public void Dispose()
    {
        if (_parameter != null)
        {
            _action(_parameter);
        }
    }
}

