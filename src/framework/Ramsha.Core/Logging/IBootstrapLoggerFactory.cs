using Microsoft.Extensions.Logging;

namespace Ramsha;

public interface IBootstrapLoggerFactory
{
    IBootstrapLogger<T> Create<T>();
    void ReplayAll(ILoggerFactory realFactory);
}
