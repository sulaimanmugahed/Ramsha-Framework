using Microsoft.Extensions.Logging;

namespace Ramsha;

public class DefaultBootstrapLogger<T> : IBootstrapLogger<T>
{
    private readonly List<BootstrapLogEntry> _entries = new();

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _entries.Add(new BootstrapLogEntry
        {
            LogLevel = logLevel,
            EventId = eventId,
            State = state!,
            Exception = exception,
            Formatter = (s, e) => formatter((TState)s, e)
        });
    }

    public void ReplayTo(ILogger<T> realLogger)
    {
        foreach (var entry in _entries)
        {
            realLogger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
        }
    }

    public void ClearEntries()
    {
        _entries.Clear();
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}
