using System.Data;

namespace Ramsha.UnitOfWork.Abstractions;

public interface IUnitOfWorkOptions
{
    bool IsTransactional { get; }
    long? Timeout { get; }
    IsolationLevel? IsolationLevel { get; }
}
