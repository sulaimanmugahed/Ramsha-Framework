

namespace Ramsha;

public interface IAppInfoAccessor
{
    string? ApplicationName { get; }
    string InstanceId { get; }
}
