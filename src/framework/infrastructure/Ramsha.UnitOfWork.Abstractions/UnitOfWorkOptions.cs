using System.Data;

namespace Ramsha.UnitOfWork.Abstractions;

public class UnitOfWorkOptions : IUnitOfWorkOptions
{
    public bool IsTransactional { get; set; }
    public long? Timeout { get; set; }
    public IsolationLevel? IsolationLevel { get; set; }


}
