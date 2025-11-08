using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;
using Ramsha.Identity.Domain;
using Ramsha.Identity.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IdentityBuilder AddRamshaIdentityEntityFrameworkCore(this IdentityBuilder builder)
    {
        RegisterServices(builder.Services, builder.UserType, builder.RoleType);
        return builder;
    }

    public static IdentityBuilder AddRamshaIdentityEntityFrameworkCore(this IdentityBuilder builder, Type userType, Type? roleType, Type userRoleType = null, Type roleClaimType = null, Type userClaimType = null, Type userLoginType = null, Type userTokenType = null)
    {
        RegisterServices(builder.Services, userType, roleType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);
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
    private static void RegisterServices(IServiceCollection services, Type userType, Type? roleType, Type userRoleType = null, Type roleClaimType = null, Type userClaimType = null, Type userLoginType = null, Type userTokenType = null)
    {
        Validate(userType, roleType);
        Type keyType = EntityHelper.FindPrimaryKeyType(userType) ?? throw new Exception("no id found for user");
        userRoleType = userRoleType ?? typeof(RamshaIdentityUserRole<>).MakeGenericType(keyType);
        roleClaimType = roleClaimType ?? typeof(RamshaIdentityRoleClaim<>).MakeGenericType(keyType);
        userClaimType = userClaimType ?? typeof(RamshaIdentityUserClaim<>).MakeGenericType(keyType);
        userLoginType = userLoginType ?? typeof(RamshaIdentityUserLogin<>).MakeGenericType(keyType);
        userTokenType = userTokenType ?? typeof(RamshaIdentityUserToken<>).MakeGenericType(keyType);

        var dbContextInterfaceType = typeof(IIdentityDbContext<,,,,,,,>)
        .MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);
        var dbContextType = typeof(IdentityDbContext<,,,,,,,>)
       .MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        RegisterDbContextAndRepositories(services, dbContextInterfaceType, dbContextType, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        AddIdentityStores(services, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

    }


    private static void AddIdentityStores(IServiceCollection services, Type userType, Type roleType, Type keyType, Type userRoleType, Type roleClaimType, Type userClaimType, Type userLoginType, Type userTokenType)
    {
        Type userStoreType = typeof(RamshaUserStore<,,,,,,,>).MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        Type roleStoreType = typeof(RamshaRoleStore<,,,,,,,>).MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

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
