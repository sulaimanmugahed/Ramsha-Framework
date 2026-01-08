

namespace Ramsha;

public interface IRamshaModuleRegistry
{
    void RegisterModule(Type moduleType, IRamshaModule instance);
    void AddDependency(Type moduleType, Type dependsOn);
    IReadOnlyList<Type> GetDependencies(Type moduleType);
    IReadOnlyList<Type> GetAllModules();
    IRamshaModule GetInstance(Type moduleType);
    bool IsRegistered(Type moduleType);
}
public class RamshaModuleRegistry : IRamshaModuleRegistry
{
    private readonly Dictionary<Type, ModuleEntry> _modules = new();
    private readonly Dictionary<Type, HashSet<Type>> _dependencies = new();

    private class ModuleEntry
    {
        public IRamshaModule Instance { get; set; } = null!;
    }

    public void RegisterModule(Type moduleType, IRamshaModule instance)
    {
        if (!_modules.ContainsKey(moduleType))
        {
            _modules[moduleType] = new ModuleEntry { Instance = instance };
            _dependencies[moduleType] = new HashSet<Type>();
        }
    }

    public void AddDependency(Type moduleType, Type dependsOn)
    {
        if (!_modules.ContainsKey(moduleType))
        {
            throw new InvalidOperationException($"Module {moduleType.Name} not registered");
        }

        _dependencies[moduleType].Add(dependsOn);
    }

    public IReadOnlyList<Type> GetDependencies(Type moduleType)
    {
        return _dependencies.TryGetValue(moduleType, out var deps)
            ? deps.ToList()
            : new List<Type>();
    }

    public IReadOnlyList<Type> GetAllModules() => _modules.Keys.ToList();

    public IRamshaModule GetInstance(Type moduleType) =>
        _modules[moduleType].Instance;

    public bool IsRegistered(Type moduleType) => _modules.ContainsKey(moduleType);
}