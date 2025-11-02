using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.UnitOfWork.Abstractions;

public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
{
    Guid Id { get; }
    Dictionary<string, object> Items { get; }
    IUnitOfWork? Outer { get; }
    IUnitOfWorkOptions Options { get; }
    void Initialize(UnitOfWorkOptions options);
    void SetOuter(IUnitOfWork? outer);
    bool IsReserved { get; }

    bool IsDisposed { get; }

    bool IsCompleted { get; }

    string? ReservationName { get; }

    void Reserve(string reservationName);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task CompleteAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
    void EnqueueLocalEvent(UoWLocalEvent @event);
}
