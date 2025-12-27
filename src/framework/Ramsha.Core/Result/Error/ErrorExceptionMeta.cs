using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ramsha;


public readonly partial record struct ErrorExceptionMeta([property: JsonIgnore] Exception Exception)
{
    /// <inheritdoc cref="Exception.Message"/>
    public string Message { get; } = Exception.Message;

    /// <inheritdoc cref="Exception.HResult"/>
    public int HResult { get; } = Exception.HResult;

    /// <inheritdoc cref="Exception.Source"/>
    public string? Source { get; } = Exception.Source;

    /// <inheritdoc cref="Exception.StackTrace"/>
    public string? StackTrace { get; } = Exception.StackTrace;

    [JsonIgnore]
    /// <inheritdoc cref="Exception.TargetSite"/>
    public MethodBase? TargetSite { get; } = Exception.TargetSite;

    /// <inheritdoc cref="Exception.Data"/>
    [JsonIgnore]
    public IDictionary? Data { get; } = Exception.Data;

    /// <summary>List of inner exceptions, if any.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ErrorExceptionMeta>? InnerExceptions { get; init; } = null;
};

partial record struct ErrorExceptionMeta
{
    /// <summary>
    /// Builds a sequence of inner exceptions from deepest to shallowest.
    /// </summary>
    public static IEnumerable<ErrorExceptionMeta> BuildInternalErrors(Exception exception)
    {
        while (exception.InnerException is not null)
        {
            exception = exception.InnerException;
            yield return new(exception);
        }
    }
}
