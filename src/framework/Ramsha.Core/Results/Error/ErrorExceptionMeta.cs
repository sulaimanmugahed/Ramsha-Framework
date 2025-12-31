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
    public string Message { get; } = Exception.Message;

    public int HResult { get; } = Exception.HResult;

    public string? Source { get; } = Exception.Source;

    public string? StackTrace { get; } = Exception.StackTrace;

    [JsonIgnore]
    public MethodBase? TargetSite { get; } = Exception.TargetSite;

    [JsonIgnore]
    public IDictionary? Data { get; } = Exception.Data;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ErrorExceptionMeta>? InnerExceptions { get; init; } = null;
};

partial record struct ErrorExceptionMeta
{
    public static IEnumerable<ErrorExceptionMeta> BuildInternalErrors(Exception exception)
    {
        while (exception.InnerException is not null)
        {
            exception = exception.InnerException;
            yield return new(exception);
        }
    }
}
