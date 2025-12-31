using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ramsha;

public interface IRamshaErrorResult : IRamshaResult
{
    /// <summary>Machine-readable error code.</summary>
    string Code { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    /// <summary>Human-readable error message.</summary>
    string? Message { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    /// <summary>Collection of errors associated with specific keys or fields.</summary>
    IEnumerable<NamedError>? Errors { get; }

    [JsonIgnore]
    /// <summary>Additional context data for diagnostics and tracing.</summary>
    RamshaErrorContext? Context { get; }
}
