namespace Ramsha;

[Serializable]
public class RamshaErrorException : RamshaException
{
    public RamshaErrorException(
        RamshaResultStatus status,
        string? message = null,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null,
        Exception? innerException = null
    )
        : base(message, innerException)
    {

        Status = status;
        Errors = errors;
        Context = context;
    }

    public IEnumerable<NamedError>? Errors { get; }
    public RamshaErrorContext? Context { get; }
    public RamshaResultStatus Status { get; }

    public RamshaErrorException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
    public RamshaErrorResult ToResult() =>
        new(
            Status: Status,
            Message: Message,
            Code: Status.ToString(),
            Errors: Errors,
            Context: Context is null
                ? new(
                    ExceptionMeta: new(this)
                    {
                        InnerExceptions = InnerException is null
                            ? null
                            : [new(InnerException)],
                    }
                )
                : Context with
                {
                    ExceptionMeta = new(this)
                    {
                        InnerExceptions = InnerException is null
                            ? Context.ExceptionMeta is null
                                ? null
                                : [Context.ExceptionMeta.Value]
                            : Context.ExceptionMeta is null
                                ? ErrorExceptionMeta.BuildInternalErrors(this)
                                :
                                [
                                    .. ErrorExceptionMeta.BuildInternalErrors(this),
                                    Context.ExceptionMeta.Value,
                                ],
                    },
                }
        );
}
