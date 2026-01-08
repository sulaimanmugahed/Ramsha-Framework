

namespace Ramsha;

public interface IModuleContainer
{
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}

public class StandAloneModuleContainer(IReadOnlyList<IModuleDescriptor> modules) : IModuleContainer
{
    IReadOnlyList<IModuleDescriptor> IModuleContainer.Modules => modules;
}