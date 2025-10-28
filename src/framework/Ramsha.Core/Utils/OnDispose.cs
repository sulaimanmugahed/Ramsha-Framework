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
