using static Ramsha.RamshaErrorsCodes;

namespace Ramsha;

public class RamshaResults
{
    public static ref SuccessResult Success() => ref SuccessResult.Value;
    public static ref NoContentResult NoContent => ref NoContentResult.Value;
    public static ValueSuccessResult<TValue> Success<TValue>(TValue value) => new(value);

    public static CreatedResult<TId> Created<TId>(TId id, string? url = null)
        where TId : IEquatable<TId>, IComparable<TId> => new(id, url);
    public static CreatedValueResult<TId, TValue> Created<TId, TValue>(
        TId id,
        TValue value,
        string? url = null
    )
        where TId : IEquatable<TId>, IComparable<TId> => new(id, value, url);

    public static AcceptedResult Accepted(JobInfo job) => new(job);

    public static RamshaErrorResult Error(
        ResultStatus status,
        string? message = null,
        string code = ERROR,
        RamshaErrorContext? context = null,
        IEnumerable<NamedError>? errors = null
    ) =>
        ((ushort)status) is < 400 or >= 2000
            ? throw new ArgumentOutOfRangeException(nameof(status))
            : new(Status: status, Message: message, Code: code, Context: context, Errors: errors);

    public static ref InvalidError Invalid()
     => ref InvalidError.Value;
    public static InvalidError Invalid(
        string message,
        string code = INVALID,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref NetworkError NetworkProblem()
     => ref NetworkError.Value;

    public static NetworkError NetworkProblem(
        string message,
        string code = NETWORK_ERROR,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref UnauthenticatedError Unauthenticated()
     => ref UnauthenticatedError.Value;
    public static UnauthenticatedError Unauthenticated(
        string message,
        string code = UNAUTHENTICATED,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref ForbiddenError Forbidden()
     => ref ForbiddenError.Value;

    public static ForbiddenError Forbidden(
        string message,
        string code = FORBIDDEN,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref PaymentRequiredError PaymentRequired()
     => ref PaymentRequiredError.Value;

    public static PaymentRequiredError PaymentRequired(
        string message,
        string code = PAYMENT_REQUIRED,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref NotFoundError NotFound()
     => ref NotFoundError.Value;

    public static NotFoundError NotFound(
        string message,
        string code = NOT_FOUND,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref TooManyRequestsError TooManyRequests()
     => ref TooManyRequestsError.Value;
    public static TooManyRequestsError TooManyRequests(
        string message,
        string code = TOO_MANY_REQUESTS,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref AbortedError Aborted()
     => ref AbortedError.Value;

    public static AbortedError Aborted(
        string message,
        string code = ABORTED,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);
    public static ref NotImplementedError NotImplemented()
     => ref NotImplementedError.Value;

    public static NotImplementedError NotImplemented(
        string message,
        string code = NOT_IMPLEMENTED,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref InsufficientStorageError InsufficientStorage()
     => ref InsufficientStorageError.Value;

    public static InsufficientStorageError InsufficientStorage(
        string message,
        string code = INSUFFICIENT_STORAGE,
        IEnumerable<NamedError>? errors = null) => new(code, message, errors);

    public static ref InternalError InternalError()
     => ref Ramsha.InternalError.Value;

    public static InternalError InternalError(
      string message,
      string code = INTERNAL_ERROR,
      IEnumerable<NamedError>? errors = null) => new(code, message, errors);

}



