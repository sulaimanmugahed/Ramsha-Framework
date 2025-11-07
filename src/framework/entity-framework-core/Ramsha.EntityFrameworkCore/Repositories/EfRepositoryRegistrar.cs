using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Domain;

namespace Ramsha.EntityFrameworkCore;



public class EfRepositoryRegistrar : RepositoryRegistrar<EfDbContextRegistrationOptions>
{
    public EfRepositoryRegistrar(EfDbContextRegistrationOptions options)
        : base(options)
    {

    }

    protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return DbContextHelper.GetEntityTypes(dbContextType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType)
    {
        return typeof(EFCoreRepository<,>).MakeGenericType(dbContextType, entityType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
    {
        return typeof(EFCoreRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
    }
}

