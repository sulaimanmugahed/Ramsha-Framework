using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Ramsha;

public class DefaultBootstrapLoggerFactory : IBootstrapLoggerFactory
{
    private readonly ConcurrentDictionary<Type, object> _loggers = new();

    public IBootstrapLogger<T> Create<T>()
        => (IBootstrapLogger<T>)_loggers.GetOrAdd(typeof(T), _ => new DefaultBootstrapLogger<T>());


    public void ReplayAll(ILoggerFactory factory)
    {
        foreach (var kvp in _loggers)
        {
            var loggerType = kvp.Key;
            var loggerInstance = kvp.Value;
            var replayMethod = loggerInstance.GetType().GetMethod("ReplayTo");
            if (replayMethod != null)
            {
                var realLogger = factory.CreateLogger(loggerType);
                replayMethod.Invoke(loggerInstance, [realLogger]);
            }
        }
    }
}
