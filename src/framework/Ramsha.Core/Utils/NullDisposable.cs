using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class NullDisposable : IDisposable
{
    public static NullDisposable Instance { get; } = new();

    private NullDisposable() { }

    public void Dispose()
    {
    }
}
