using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Shared;

namespace Ramsha.Identity.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRamshaIdentityApplicationServices(this IServiceCollection services)
    {
        var entityTypes = services.ExecutePreConfigured<RamshaIdentityTypesOptions>();
        var contractsOptions = services.ExecutePreConfigured<RamshaIdentityContractsOptions>();

        var userServiceImplementation = contractsOptions.ReplacedUserServiceType is not null ? contractsOptions.ReplacedUserServiceType : typeof(RamshaIdentityUserService<,,,,,,,,,,>).MakeGenericType(
            entityTypes.UserType,
            entityTypes.RoleType,
            entityTypes.KeyType,
            entityTypes.UserRoleType,
            entityTypes.RoleClaimType,
            entityTypes.UserClaimType,
            entityTypes.UserLoginType,
            entityTypes.UserTokenType,
           contractsOptions.GetReplacedDtoOrBase<RamshaIdentityUserDto>(),
            contractsOptions.GetReplacedDtoOrBase<CreateRamshaIdentityUserDto>(),
            contractsOptions.GetReplacedDtoOrBase<UpdateRamshaIdentityUserDto>()

        );
        var userServiceInterface = typeof(IRamshaIdentityUserService<,,,>)
        .MakeGenericType(
           contractsOptions.GetReplacedDtoOrBase<RamshaIdentityUserDto>(),
           contractsOptions.GetReplacedDtoOrBase<CreateRamshaIdentityUserDto>(),
            contractsOptions.GetReplacedDtoOrBase<UpdateRamshaIdentityUserDto>(),
            entityTypes.KeyType
            );

        services.AddRamshaService(userServiceImplementation, userServiceInterface);


        var roleServiceImplementation = contractsOptions.ReplacedRoleServiceType is not null ? contractsOptions.ReplacedRoleServiceType : typeof(RamshaIdentityRoleService<,,,,,,>).MakeGenericType(
            entityTypes.RoleType,
            entityTypes.KeyType,
            entityTypes.UserRoleType,
            entityTypes.RoleClaimType,
contractsOptions.GetReplacedDtoOrBase<RamshaIdentityRoleDto>(),
            contractsOptions.GetReplacedDtoOrBase<CreateRamshaIdentityRoleDto>(),
            contractsOptions.GetReplacedDtoOrBase<UpdateRamshaIdentityRoleDto>()

        );
        var roleServiceInterface = typeof(IRamshaIdentityRoleService<,,,>)
        .MakeGenericType(
contractsOptions.GetReplacedDtoOrBase<RamshaIdentityRoleDto>(),
           contractsOptions.GetReplacedDtoOrBase<CreateRamshaIdentityRoleDto>(),
            contractsOptions.GetReplacedDtoOrBase<UpdateRamshaIdentityRoleDto>(),
            entityTypes.KeyType
            );

        services.AddRamshaService(roleServiceImplementation, roleServiceInterface);

        return services;
    }
}
