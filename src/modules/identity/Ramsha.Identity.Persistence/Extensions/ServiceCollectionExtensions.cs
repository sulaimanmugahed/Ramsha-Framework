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
    public static IdentityBuilder AddRamshaIdentityStore<TDbContext>(this IdentityBuilder builder)
    where TDbContext : IEFDbContext
    {
        AddStores(builder.Services, typeof(TDbContext), builder.UserType, builder.RoleType);
        return builder;
    }

    public static IdentityBuilder AddRamshaIdentityStore<TDbContext, TId>(this IdentityBuilder builder)
    where TDbContext : IEFDbContext

    {
        AddStores(builder.Services, typeof(TDbContext), builder.UserType, builder.RoleType, typeof(TId));
        return builder;
    }

    internal static IdentityBuilder AddRamshaIdentityStore(this IdentityBuilder builder, Type dbContextType, Type userType, Type roleType, Type keyType = null, Type userRoleType = null, Type roleClaimType = null, Type userClaimType = null, Type userLoginType = null, Type userTokenType = null)
    {
        AddStores(builder.Services, dbContextType, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);
        return builder;
    }


    public static IdentityBuilder AddRamshaIdentityStore<TDbContext, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(this IdentityBuilder builder)
    where TDbContext : IEFDbContext
    {
        AddStores(builder.Services, typeof(TDbContext), builder.UserType, builder.RoleType, typeof(TId), typeof(TUserRole), typeof(TRoleClaim), typeof(TUserClaim), typeof(TUserLogin), typeof(TUserToken));
        return builder;
    }



    private static void AddStores(IServiceCollection services, Type dbContextType, Type userType, Type roleType, Type keyType = null, Type userRoleType = null, Type roleClaimType = null, Type userClaimType = null, Type userLoginType = null, Type userTokenType = null)
    {
        keyType = keyType ?? EntityHelper.FindPrimaryKeyType(userType) ?? typeof(Guid);
        userRoleType = userRoleType ?? typeof(RamshaIdentityUserRole<>).MakeGenericType(keyType);
        roleClaimType = roleClaimType ?? typeof(RamshaIdentityRoleClaim<>).MakeGenericType(keyType);
        userClaimType = userClaimType ?? typeof(RamshaIdentityUserClaim<>).MakeGenericType(keyType);
        userLoginType = userLoginType ?? typeof(RamshaIdentityUserLogin<>).MakeGenericType(keyType);
        userTokenType = userTokenType ?? typeof(RamshaIdentityUserToken<>).MakeGenericType(keyType);

        Type userStoreType = typeof(RamshaUserStore<,,,,,,,>).MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        Type roleStoreType = typeof(RamshaRoleStore<,,,,,,,>).MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);


        Type roleRepositoryInterfaceType = typeof(IIdentityRoleRepository<,,,,,,,>).MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);
        Type roleRepositoryType = typeof(EFIdentityRoleRepository<,,,,,,,,>).MakeGenericType(dbContextType, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        Type userRepositoryInterfaceType = typeof(IIdentityUserRepository<,,,,,,,>).MakeGenericType(userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);
        Type userRepositoryType = typeof(EFIdentityUserRepository<,,,,,,,,>).MakeGenericType(dbContextType, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);

        services.RegisterCustomRepository(roleRepositoryInterfaceType, roleRepositoryType);
        services.RegisterCustomRepository(userRepositoryInterfaceType, userRepositoryType);


        // replace the default repositories (ex: IRepository<RamshaIdentityUser,Guid>) for user and role
        services.RegisterDefaultRepository(roleType, roleRepositoryType, replaceExisting: true);
        services.RegisterDefaultRepository(userType, userRepositoryType, replaceExisting: true);


        services.AddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        services.AddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
    }


}
