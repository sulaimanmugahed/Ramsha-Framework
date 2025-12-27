

namespace Ramsha;

public static class RamshaErrorsCodes
{
    public const string NOT_FOUND = nameof(ResultStatus.NotFound);
    public const string ERROR = nameof(ResultStatus.Unknown);
    public const string NETWORK_ERROR = nameof(ResultStatus.NetworkError);
    public const string UNEXPECTED_ERROR = nameof(ResultStatus.Unexpected);
    public const string ABORTED = nameof(ResultStatus.Aborted);
    public const string UNAUTHENTICATED = nameof(ResultStatus.Unauthenticated);
    public const string FORBIDDEN = nameof(ResultStatus.Forbidden);
    public const string PAYMENT_REQUIRED = nameof(ResultStatus.PaymentRequired);
    public const string INVALID = nameof(ResultStatus.Invalid);
    public const string UNPROCESSABLE_DATA = nameof(ResultStatus.UnprocessableData);
    public const string CONFLICT = nameof(ResultStatus.Conflict);
    public const string WRONG_CONTEXT = nameof(ResultStatus.WrongContext);
    public const string TOO_MANY_REQUESTS = nameof(ResultStatus.TooManyRequests);
    public const string INTERNAL_ERROR = nameof(ResultStatus.InternalError);
    public const string SERVICE_UNAVAILABLE = nameof(ResultStatus.CriticalServiceUnavailable);
    public const string NOT_IMPLEMENTED = nameof(ResultStatus.CriticalNotImplemented);
    public const string INSUFFICIENT_STORAGE = nameof(ResultStatus.CriticalInsufficientStorage);
    public const string TIMEOUT = nameof(ResultStatus.CriticalTimeout);

}
