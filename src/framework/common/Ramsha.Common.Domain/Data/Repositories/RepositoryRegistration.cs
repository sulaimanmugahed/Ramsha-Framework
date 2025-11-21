using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Common.Domain;

public class CustomRepositoryRegistration
{
    public CustomRepositoryRegistration(Type repositoryType, bool selfRegister)
    {
        SelfRegister = selfRegister;
        RepositoryType = repositoryType;
    }

    public CustomRepositoryRegistration AddInterface(Type interfaceType)
    {
        if (!RepositoryInterfacesTypes.Any(x => x == interfaceType))
        {
            RepositoryInterfacesTypes.Add(interfaceType);
        }
        return this;
    }

    public bool SelfRegister { get; set; }
    public Type RepositoryType { get; set; }
    public List<Type> RepositoryInterfacesTypes { get; set; } = [];
}
