using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;

public class AppModulesContext
{

    private readonly Dictionary<Type, List<Type>> _modulesTypes = new();

    public AppModulesContext()
    {
    }


    public void InsureModuleExist(Type moduleType)
    {
        if (!_modulesTypes.ContainsKey(moduleType))
            _modulesTypes[moduleType] = new List<Type>();
    }

    public void AddDependency(Type moduleType, Type dependsOn)
    {
        InsureModuleExist(moduleType);
        if (!_modulesTypes[moduleType].Contains(dependsOn))
            _modulesTypes[moduleType].Add(dependsOn);
    }

    public IEnumerable<Type> GetDependenciesTypes(Type moduleType)
    {
        return _modulesTypes.TryGetValue(moduleType, out var deps)
            ? deps
            : Type.EmptyTypes;
    }

    public IEnumerable<Type> GetAllModulesTypes()
    {
        return _modulesTypes.Keys;
    }

    public List<Type> GetAllModulesTypesOrdered(Type startupModuleType)
    {
        return _modulesTypes.Keys.ToList();
    }

    public IEnumerable<Type> GetAllModulesTypesOrdered<StartupModuleType>()
    {
        return GetAllModulesTypesOrdered(typeof(StartupModuleType));
    }
}
