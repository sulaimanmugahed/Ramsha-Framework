using Microsoft.Extensions.DependencyInjection;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork.Abstractions;
using static Ramsha.RamshaErrorsCodes;


namespace Ramsha.LocalMessaging;

public abstract class CommandHandler<TCommand, TResult> : IRamshaCommandHandler<TCommand, TResult>
where TCommand : IRamshaCommand<TResult>
{
    [Injectable]
    public IServiceProvider ServiceProvider { get; set; } = default!;
    protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetLazyRequiredService<IUnitOfWorkManager>().Value;

    protected Task<T> TransactionalUnitOfWork<T>(Func<Task<T>> action)
    {
        return UnitOfWork(action, true);
    }
    protected Task TransactionalUnitOfWork(Func<Task> action)
    {
        return UnitOfWork(action, true);
    }

    protected async Task<T> UnitOfWork<T>(Func<Task<T>> action, bool isTransactional = false)
    {
        var options = new UnitOfWorkOptions();
        options.IsTransactional = isTransactional;

        if (UnitOfWorkManager.TryBeginReserved(
                RamshaUnitOfWorkReservationNames.ActionUnitOfWorkReservationName,
                options))
        {
            var result = await action();
            if (UnitOfWorkManager.Current is not null)
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            return result;
        }

        using (var uow = UnitOfWorkManager.Begin(options))
        {
            var result = await action();
            await uow.CompleteAsync();
            return result;
        }
    }

    protected async Task UnitOfWork(Func<Task> action, bool isTransactional = false)
    {
        var options = new UnitOfWorkOptions();
        options.IsTransactional = isTransactional;

        if (UnitOfWorkManager.TryBeginReserved(
                RamshaUnitOfWorkReservationNames.ActionUnitOfWorkReservationName,
                options))
        {
            await action();
            if (UnitOfWorkManager.Current is not null)
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            return;
        }

        using (var uow = UnitOfWorkManager.Begin(options))
        {
            await action();
            await uow.CompleteAsync();
        }
    }
    public abstract Task<TResult> HandleAsync(TCommand message, CancellationToken cancellationToken = default);
}

public abstract class CommandHandler<TCommand> : CommandHandler<TCommand, IRamshaResult>
where TCommand : IRamshaCommand<IRamshaResult>
{
    protected NoContentResult NoContent => RamshaResults.NoContent;

    protected SuccessResult Success()
    => RamshaResults.Success();

    protected ValueSuccessResult<TValue> Success<TValue>(TValue value)
        => RamshaResults.Success(value);

    protected CreatedResult<TId> Created<TId>(TId id, string? url = null)
    where TId : IEquatable<TId>, IComparable<TId> => RamshaResults.Created(id, url);

    protected CreatedValueResult<TId, TValue> Created<TId, TValue>(
        TId id,
        TValue value,
        string? url = null)
    where TId : IEquatable<TId>, IComparable<TId> => RamshaResults.Created(id, value, url);

    protected AcceptedResult Accepted(JobInfo job) => RamshaResults.Accepted(job);

    protected NotFoundError NotFound()
        => RamshaResults.NotFound();

    protected NotFoundError NotFound(string message, string code = NOT_FOUND, IEnumerable<NamedError>? errors = null)
        => RamshaResults.NotFound(message, code, errors);

    protected InvalidError Invalid()
    => RamshaResults.Invalid();

    protected InvalidError Invalid(string message, string code = INVALID, IEnumerable<NamedError>? errors = null)
        => RamshaResults.Invalid(message, code, errors);

    protected NetworkError NetworkProblem()
        => RamshaResults.NetworkProblem();

    protected NetworkError NetworkProblem(string message, string code = NETWORK_ERROR, IEnumerable<NamedError>? errors = null)
        => RamshaResults.NetworkProblem(message, code, errors);

    protected UnauthenticatedError Unauthenticated()
        => RamshaResults.Unauthenticated();

    protected UnauthenticatedError Unauthenticated(string message, string code = UNAUTHENTICATED, IEnumerable<NamedError>? errors = null)
        => RamshaResults.Unauthenticated(message, code, errors);

    protected ForbiddenError Forbidden()
        => RamshaResults.Forbidden();

    protected ForbiddenError Forbidden(string message, string code = FORBIDDEN, IEnumerable<NamedError>? errors = null)
        => RamshaResults.Forbidden(message, code, errors);

    protected PaymentRequiredError PaymentRequired()
        => RamshaResults.PaymentRequired();

    protected PaymentRequiredError PaymentRequired(string message, string code = PAYMENT_REQUIRED, IEnumerable<NamedError>? errors = null)
        => RamshaResults.PaymentRequired(message, code, errors);

    protected AbortedError Aborted()
        => RamshaResults.Aborted();

    protected AbortedError Aborted(string message, string code = ABORTED, IEnumerable<NamedError>? errors = null)
        => RamshaResults.Aborted(message, code, errors);

    protected NotImplementedError NotImplemented()
        => RamshaResults.NotImplemented();

    protected NotImplementedError NotImplemented(string message, string code = NOT_IMPLEMENTED, IEnumerable<NamedError>? errors = null)
        => RamshaResults.NotImplemented(message, code, errors);

    protected InsufficientStorageError InsufficientStorage()
        => RamshaResults.InsufficientStorage();

    protected InsufficientStorageError InsufficientStorage(string message, string code = INSUFFICIENT_STORAGE, IEnumerable<NamedError>? errors = null)
        => RamshaResults.InsufficientStorage(message, code, errors);

    protected InternalError InternalError()
        => RamshaResults.InternalError();

    protected InternalError InternalError(string message, string code = INTERNAL_ERROR, IEnumerable<NamedError>? errors = null)
        => RamshaResults.InternalError(message, code, errors);
}

