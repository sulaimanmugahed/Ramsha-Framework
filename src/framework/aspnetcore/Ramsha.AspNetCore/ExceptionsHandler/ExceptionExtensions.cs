using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ramsha.RamshaErrorsCodes;

namespace Ramsha.AspNetCore;

public static class ExceptionExtensions
{
    public static InternalError ToInternalError(
        this Exception _,
        string? message = null,
        string code = INTERNAL_ERROR,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null
    ) => new(Message: message, Code: code, Errors: errors, Context: context);


    public static RamshaErrorResult ToKnownError(
        this Exception ex,
        string? message = null,
        IEnumerable<NamedError>? errors = null,
        RamshaErrorContext? context = null
    )
    {
        if (ex is RamshaErrorException resultException)
            return resultException.ToResult();

        return ex.ToInternalError(message: message, errors: errors, context: context);
    }
}
