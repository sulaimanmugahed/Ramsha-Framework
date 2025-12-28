using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ramsha.RamshaErrorsCodes;

namespace Ramsha.AspNetCore;

public static class ExceptionExtensions
{
    public static ref AbortedError ToAbortedError(this OperationCanceledException _) =>
       ref AbortedError.Value;

    /// <summary>
    /// Creates an aborted error for <see cref="OperationCanceledException"/> with optional details.
    /// </summary>
    public static AbortedError ToAbortedError(
        this OperationCanceledException _,
        string? message = null,
        string code = ABORTED,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null
    ) => new(Message: message, Code: code, Errors: errors, Context: context);

    /// <summary>
    /// Gets a cached not-implemented error instance for <see cref="NotImplementedException"/>.
    /// </summary>
    public static ref NotImplementedError ToNotImplementedError(this NotImplementedException _) =>
        ref NotImplementedError.Value;

    /// <summary>
    /// Creates a not-implemented error for <see cref="NotImplementedException"/> with optional details.
    /// </summary>
    public static NotImplementedError ToNotImplementedError(
        this NotImplementedException _,
        string? message = null,
        string code = NOT_IMPLEMENTED,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null
    ) => new(Message: message, Code: code, Errors: errors, Context: context);

    /// <summary>
    /// Creates an internal error for unknown exceptions.
    /// </summary>
    public static InternalError ToInternalError(
        this Exception _,
        string? message = null,
        string code = INTERNAL_ERROR,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null
    ) => new(Message: message, Code: code, Errors: errors, Context: context);

    /// <summary>
    /// Converts known exceptions into standardized error results. Falls back to internal error.
    /// </summary>
    public static IRamshaErrorResult ToKnownError(
        this Exception ex,
        string? message = null,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null
    )
    {
        if (ex is RamshaErrorException resultException)
            return resultException.ToResult();

        if (ex is OperationCanceledException cancelled)
            return cancelled.ToAbortedError(message: message, errors: errors, context: context);

        if (ex is NotImplementedException notImplemented)
            return notImplemented.ToNotImplementedError(
                message: message,
                errors: errors,
                context: context
            );

        return ex.ToInternalError(message: message, errors: errors, context: context);
    }
}
