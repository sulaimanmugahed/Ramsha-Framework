using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

[Serializable]
/// <summary>
/// Exception type used to propagate domain or validation errors with rich context that can be
/// transformed to an <see cref="ErrorResult"/> via <see cref="ToResult"/>.
/// </summary>
public class RamshaErrorException : Exception
{
    /// <summary>
    /// Creates a new <see cref="ErrorException"/> for client-side processing errors.
    /// Only certain <see cref="ResultStatus"/> values are supported.
    /// </summary>
    /// <param name="reason">The reason status (e.g., InvalidData, UnprocessableData, WrongContext, Conflict, NotFound).</param>
    /// <param name="message">Optional error message.</param>
    /// <param name="errors">Optional list of named errors.</param>
    /// <param name="context">Optional error context to aid diagnostics.</param>
    /// <param name="innerException">Optional inner exception.</param>
    public RamshaErrorException(
        ResultStatus reason,
        string? message = null,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null,
        Exception? innerException = null
    )
        : base(message, innerException)
    {
        if (
            reason
            is not ResultStatus.Invalid
                and not ResultStatus.UnprocessableData
                and not ResultStatus.WrongContext
                and not ResultStatus.Conflict
                and not ResultStatus.NotFound
        )
            throw new ArgumentOutOfRangeException(
                nameof(reason),
                $"ResultStatus ({reason}) value is not supported."
            );
        this.Reason = reason;

        this.Errors = errors;
        this.Context = context;
    }

    /// <summary>Collection of named errors associated with this exception.</summary>
    public IEnumerable<NamedError>? Errors { get; }

    /// <summary>Context information for diagnostics and tracing.</summary>
    public RamshaErrorContext? Context { get; }

    /// <summary>The result status representing the error reason.</summary>
    public ResultStatus Reason { get; }

    /// <summary>
    /// Converts this exception into an <see cref="ErrorResult"/>, enriching the
    /// <see cref="ErrorContext"/> with exception metadata when available.
    /// </summary>
    /// <returns>The corresponding <see cref="ErrorResult"/>.</returns>
    public RamshaErrorResult ToResult() =>
        new(
            Status: this.Reason,
            Message: this.Message,
            Code: this.Reason.ToString(),
            Errors: Errors,
            Context: Context is null
                ? new(
                    ExceptionMeta: new(this)
                    {
                        InnerExceptions = this.InnerException is null
                            ? null
                            : [new(this.InnerException)],
                    }
                )
                : Context with
                {
                    ExceptionMeta = new(this)
                    {
                        InnerExceptions = this.InnerException is null
                            ? this.Context.ExceptionMeta is null
                                ? null
                                : [this.Context.ExceptionMeta.Value]
                            : this.Context.ExceptionMeta is null
                                ? ErrorExceptionMeta.BuildInternalErrors(this)
                                :
                                [
                                    .. ErrorExceptionMeta.BuildInternalErrors(this),
                                    this.Context.ExceptionMeta.Value,
                                ],
                    },
                }
        );
}
