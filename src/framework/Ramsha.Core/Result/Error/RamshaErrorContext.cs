

namespace Ramsha;

public record RamshaErrorContext(
        string? CorrelationId = null,
    ResultActivityMeta? ActivityMeta = null,
    ErrorExceptionMeta? ExceptionMeta = null,
    string? SourceName = null,
    object? Source = null,
    object? Sender = null,
    object? Data = null
);

