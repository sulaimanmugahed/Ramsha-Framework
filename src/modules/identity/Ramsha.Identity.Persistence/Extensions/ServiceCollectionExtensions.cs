using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Shared;
using Ramsha.Identity.Domain;
using Ramsha.Identity.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IdentityBuilder AddRamshaIdentityEntityFrameworkCore(this IdentityBuilder builder)
    {
        RegisterServices(builder.Services);
        return builder;
    }

    private static void RegisterDbContextAndRepositories(IServiceCollection services, Type dbContextInterfaceType, Type dbContextType, Type userType, Type roleType, Type keyType, Type userRoleType, Type roleClaimType, Type userClaimType, Type userLoginType, Type userTokenType)
    {
        services.AddRamshaDbContext(dbContextInterfaceType, dbContextType);

        Type roleRepositoryInterfaceType = typeof(IIdentityRoleRepository<,>).MakeGenericType(roleType, keyType);
        Type roleRepositoryType = typeof(EFIdentityRoleRepository<,,,,,,,,>).MakeGenericType(dbContextInterfaceType, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        Type userRepositoryInterfaceType = typeof(IIdentityUserRepository<,>).MakeGenericType(userType, keyType);
        Type userRepositoryType = typeof(EFIdentityUserRepository<,,,,,,,,>).MakeGenericType(dbContextInterfaceType, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        services.RegisterCustomRepository(roleRepositoryInterfaceType, roleRepositoryType);
        services.RegisterCustomRepository(userRepositoryInterfaceType, userRepositoryType);


        // replace the default repositories (ex: IRepository<RamshaIdentityUser,Guid>) for user and role
        services.RegisterDefaultRepository(roleType, roleRepositoryType, replaceExisting: true);
        services.RegisterDefaultRepository(userType, userRepositoryType, replaceExisting: true);

    }
    private static void RegisterServices(IServiceCollection services)
    {
        var typesOptions = services.ExecutePreConfigured<RamshaIdentityTypesOptions>();
        Validate(typesOptions.UserType, typesOptions.RoleType);


        var dbContextInterfaceType = typeof(IIdentityDbContext<,,,,,,,>)
        .MakeGenericType(typesOptions.UserType, typesOptions.RoleType, typesOptions.KeyType, typesOptions.UserRoleType, typesOptions.RoleClaimType, typesOptions.UserClaimType, typesOptions.UserLoginType, typesOptions.UserTokenType);
        var dbContextType = typeof(IdentityDbContext<,,,,,,,>)
       .MakeGenericType(typesOptions.UserType, typesOptions.RoleType, typesOptions.KeyType, typesOptions.UserRoleType, typesOptions.RoleClaimType, typesOptions.UserClaimType, typesOptions.UserLoginType, typesOptions.UserTokenType);

        RegisterDbContextAndRepositories(services, dbContextInterfaceType, dbContextType, typesOptions.UserType, typesOptions.RoleType, typesOptions.KeyType, typesOptions.UserRoleType, typesOptions.RoleClaimType, typesOptions.UserClaimType, typesOptions.UserLoginType, typesOptions.UserTokenType);

        AddIdentityStores(services, typesOptions.UserType, typesOptions.RoleType, typesOptions.KeyType, typesOptions.UserRoleType, typesOptions.RoleClaimType, typesOptions.UserClaimType, typesOptions.UserLoginType, typesOptions.UserTokenType);

    }


    private static void AddIdentityStores(IServiceCollection services, Type userType, Type roleType, Type keyType, Type userRoleType, Type roleClaimType, Type userClaimType, Type userLoginType, Type userTokenType)
    {
        Type userStoreType = typeof(RamshaIdentityUserStore<,,,,,,,>).MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        Type roleStoreType = typeof(RamshaIdentityRoleStore<,,,>).MakeGenericType(roleType, keyType, userRoleType, roleClaimType);

        services.AddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        services.AddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
    }

    private static void Validate(Type? userType, Type? roleType)
    {
        if (!typeof(IHasId).IsAssignableFrom(userType))
        {
            throw new Exception("invalid user type");
        }

        if (roleType is null)
        {
            throw new Exception("No role type selected for identity");
        }

    }


}
