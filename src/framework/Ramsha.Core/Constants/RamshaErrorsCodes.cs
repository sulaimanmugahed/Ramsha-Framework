

namespace Ramsha;

public static class RamshaErrorsCodes
{
    public const string NOT_FOUND = nameof(RamshaResultStatus.NotFound);
    public const string ERROR = nameof(RamshaResultStatus.Unknown);
    public const string NETWORK_ERROR = nameof(RamshaResultStatus.NetworkError);
    public const string UNEXPECTED_ERROR = nameof(RamshaResultStatus.Unexpected);
    public const string ABORTED = nameof(RamshaResultStatus.Aborted);
    public const string UNAUTHENTICATED = nameof(RamshaResultStatus.Unauthenticated);
    public const string FORBIDDEN = nameof(RamshaResultStatus.Forbidden);
    public const string PAYMENT_REQUIRED = nameof(RamshaResultStatus.PaymentRequired);
    public const string INVALID = nameof(RamshaResultStatus.Invalid);
    public const string UNPROCESSABLE_DATA = nameof(RamshaResultStatus.UnprocessableData);
    public const string CONFLICT = nameof(RamshaResultStatus.Conflict);
    public const string WRONG_CONTEXT = nameof(RamshaResultStatus.WrongContext);
    public const string TOO_MANY_REQUESTS = nameof(RamshaResultStatus.TooManyRequests);
    public const string INTERNAL_ERROR = nameof(RamshaResultStatus.InternalError);
    public const string SERVICE_UNAVAILABLE = nameof(RamshaResultStatus.CriticalServiceUnavailable);
    public const string NOT_IMPLEMENTED = nameof(RamshaResultStatus.CriticalNotImplemented);
    public const string INSUFFICIENT_STORAGE = nameof(RamshaResultStatus.CriticalInsufficientStorage);
    public const string TIMEOUT = nameof(RamshaResultStatus.CriticalTimeout);

}
