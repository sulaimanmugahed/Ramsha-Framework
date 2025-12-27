using System.Net;

namespace Ramsha;

public enum ResultStatus : ushort
{
    _ = 999,
    Unknown = _,
    NetworkError = 0,
    Error,

    Aborted = 499,
    Unexpected = 599,

    //==============
    // Success codes

    /// <summary>
    /// Equivalent to HTTP status 200. <see cref="OK"/> indicates that the
    /// request succeeded and that the requested information is in the response.
    /// </summary>
    OK = HttpStatusCode.OK,

    /// <summary>
    /// Equivalent to HTTP status 201. <see cref="Created"/> indicates that
    /// the request resulted in a new resource created before the response was sent.
    /// </summary>
    Created = HttpStatusCode.Created,

    /// <summary>
    /// Equivalent to HTTP status 204. <see cref="NoContent"/> indicates
    /// that the request has been successfully processed, and that the response is intentionally blank.
    /// </summary>
    NoContent = HttpStatusCode.NoContent,

    /// <summary>
    /// Equivalent to HTTP status 202. <see cref="Accepted"/> indicates that
    /// the request has been accepted for further processing.
    /// </summary>
    Accepted = HttpStatusCode.Accepted,

    //====================================
    // Authentication-related status codes

    /// <summary>
    /// User is unauthenticated; requires login.
    /// </summary>
    Unauthenticated = HttpStatusCode.Unauthorized,

    /// <summary>
    /// User is authenticated but lacks permissions for the request.
    /// </summary>
    Forbidden = HttpStatusCode.Forbidden,

    /// <summary>
    /// Payment is required to access the requested resource.
    /// </summary>
    PaymentRequired = HttpStatusCode.PaymentRequired,

    //===================
    // Client error codes
    // Managed by ASP.NET and Kestrel without custom handling.

    /// <summary>
    /// Client sent a malformed or invalid request.
    /// </summary>
    Invalid = HttpStatusCode.BadRequest,

    /// <summary>
    /// Requested resource could not be found.
    /// </summary>
    NotFound = HttpStatusCode.NotFound,

    /// <summary>
    /// Client has made too many requests in a given timeframe.
    /// </summary>
    TooManyRequests = HttpStatusCode.TooManyRequests,

    //=======================
    // Processing error codes

    /// <summary>
    /// A conflict occurred during processing, e.g., a versioning conflict, or removal failed for dependency.
    /// </summary>
    Conflict = HttpStatusCode.Conflict,

    /// <summary>
    /// Request is syntactically correct but semantically invalid or unprocessable.
    /// </summary>
    UnprocessableData = HttpStatusCode.UnprocessableEntity,

    /// <summary>
    /// This is a teapot. There's no coffee here.
    /// </summary>
    Teapot = 418,

    /// <summary>
    /// Request reached the wrong processor or context.
    /// </summary>
    WrongContext = Teapot,

    //==================
    // Critical error codes

    /// <summary>
    /// An internal server error occurred during processing.
    /// </summary>
    InternalError = HttpStatusCode.InternalServerError,

    /// <summary>
    /// Processing timeout or delay exceeded permissible limits.
    /// </summary>
    CriticalTimeout = HttpStatusCode.GatewayTimeout,

    /// <summary>
    /// Server lacks sufficient storage to complete the request.
    /// </summary>
    CriticalInsufficientStorage = HttpStatusCode.InsufficientStorage,

    /// <summary>
    /// Server currently not available due to high load or maintenance.
    /// </summary>
    CriticalServiceUnavailable = HttpStatusCode.ServiceUnavailable,

    /// <summary>
    /// The operation process is partially implemented or depending on a non-implemented feature.
    /// </summary>
    CriticalNotImplemented = HttpStatusCode.NotImplemented,
}
