using Microsoft.Extensions.Logging;

namespace Ramsha;

public interface IBootstrapLogger<T> : ILogger<T>
{
    void ReplayTo(ILogger<T> realLogger);
    void ClearEntries();
}
