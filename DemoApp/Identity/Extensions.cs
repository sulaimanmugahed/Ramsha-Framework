using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ramsha.Domain;

namespace DemoApp.Identity;

public static class ServiceCollectionExtensions
{

    public static IdentityBuilder AddRamshaIdentity(this IdentityBuilder builder)
    {

        AddStores(builder.Services, builder.UserType, builder.RoleType);
        return builder;
    }

    public static IdentityBuilder AddRamshaIdentity<TId>(this IdentityBuilder builder)
    {
        AddStores(builder.Services, builder.UserType, builder.RoleType, typeof(TId));
        return builder;
    }

    internal static IdentityBuilder AddRamshaIdentity(this IdentityBuilder builder, Type userType, Type roleType, Type keyType = null, Type userRoleType = null, Type roleClaimType = null, Type userClaimType = null, Type userLoginType = null, Type userTokenType = null)
    {
        AddStores(builder.Services, userType, roleType, keyType, userRoleType, roleClaimType, userClaimType, userLoginType, userTokenType);
        return builder;
    }


    public static IdentityBuilder AddRamshaIdentity<TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken>(this IdentityBuilder builder)
    {
        AddStores(builder.Services, builder.UserType, builder.RoleType, typeof(TId), typeof(TUserRole), typeof(TRoleClaim), typeof(TUserClaim), typeof(TUserLogin), typeof(TUserToken));
        return builder;
    }



    private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type keyType = null, Type userRoleType = null, Type roleClaimType = null, Type userClaimType = null, Type userLoginType = null, Type userTokenType = null)
    {
        keyType = keyType ?? EntityHelper.FindPrimaryKeyType(userType) ?? typeof(Guid);
        userRoleType = userRoleType ?? typeof(RamshaIdentityUserRole<>).MakeGenericType(keyType);
        roleClaimType = roleClaimType ?? typeof(RamshaIdentityRoleClaim<>).MakeGenericType(keyType);
        userClaimType = userClaimType ?? typeof(RamshaIdentityUserClaim<>).MakeGenericType(keyType);
        userLoginType = userLoginType ?? typeof(RamshaIdentityUserLogin<>).MakeGenericType(keyType);
        userTokenType = userTokenType ?? typeof(RamshaIdentityUserToken<>).MakeGenericType(keyType);

        Type userStoreType = typeof(RamshaUserStore<,,,,,,,>).MakeGenericType(userType, keyType, userRoleType, roleClaimType,
                                                                        userClaimType, userLoginType, userTokenType, roleType);

        Type roleStoreType = typeof(RamshaRoleStore<,,,>).MakeGenericType(roleType, keyType, userRoleType, roleClaimType);


        Type roleRepositoryInterfaceType = typeof(IIdentityRoleRepository<,,,>).MakeGenericType(roleType, keyType, userRoleType, roleClaimType);
        Type roleRepositoryType = typeof(IdentityRoleRepository<,,,>).MakeGenericType(roleType, keyType, userRoleType, roleClaimType);

        Type userRepositoryInterfaceType = typeof(IIdentityUserRepository<,,,,,,,>).MakeGenericType(userType, keyType, userRoleType,
                                                                           roleClaimType, userClaimType,
                                                                           userLoginType, userTokenType, roleType);
        Type userRepositoryType = typeof(IdentityUserRepository<,,,,,,,>).MakeGenericType(userType, keyType, userRoleType,
                                                                          roleClaimType, userClaimType,
                                                                          userLoginType, userTokenType, roleType);

        services.RegisterCustomRepository(roleRepositoryInterfaceType, roleRepositoryType);
        services.RegisterCustomRepository(userRepositoryInterfaceType, userRepositoryType);


        // replace the default repositories (ex: IRepository<RamshaIdentityUser,Guid>) for user and role
        services.RegisterDefaultRepository(roleType, roleRepositoryType, replaceExisting: true);
        services.RegisterDefaultRepository(userType, userRepositoryType, replaceExisting: true);


        services.AddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        services.AddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
    }


}