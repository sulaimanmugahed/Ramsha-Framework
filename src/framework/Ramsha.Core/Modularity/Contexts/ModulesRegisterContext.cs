using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ramsha;

public class RegisterContext
{
    private readonly IServiceCollection _services;
    private Type? CurrentModule { get; set; }

    private readonly Dictionary<Type, List<Type>> _dependenciesMap = new();

    public RegisterContext(IServiceCollection services)
    {
        _services = services;
    }

    internal void SetCurrentModule(IRamshaModule module)
    {
        CurrentModule = module.GetType();
        EnsureModuleEntry(CurrentModule);
    }

    public RegisterContext DependsOn<TModule>() where TModule : IRamshaModule
        => DependsOn(typeof(TModule));

    public RegisterContext DependsOn(Type type)
    {
        if (CurrentModule is null)
            throw new InvalidOperationException("No current module is set.");

        EnsureModuleEntry(type);
        AddDependency(CurrentModule, type);

        return this;
    }

    private void EnsureModuleEntry(Type moduleType)
    {
        if (!_dependenciesMap.ContainsKey(moduleType))
            _dependenciesMap[moduleType] = new List<Type>();
    }

    private void AddDependency(Type moduleType, Type dependsOn)
    {
        EnsureModuleEntry(moduleType);

        if (!_dependenciesMap[moduleType].Contains(dependsOn))
            _dependenciesMap[moduleType].Add(dependsOn);
    }

    public IReadOnlyList<Type> GetDependenciesTypes(Type moduleType)
        => _dependenciesMap.TryGetValue(moduleType, out var deps) ? deps : Type.EmptyTypes;

    public IReadOnlyList<Type> GetAllModulesTypes() => _dependenciesMap.Keys.ToList();
}
