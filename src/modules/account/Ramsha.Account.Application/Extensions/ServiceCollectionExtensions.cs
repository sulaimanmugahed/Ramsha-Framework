

using Ramsha.Account.Application;
using Ramsha.Account.Contracts;
using Ramsha.Identity.Shared;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountApplicationServices(this IServiceCollection services)
    {
        var entityTypes = services.ExecutePreparedOptions<RamshaIdentityTypesOptions>();
        var contractsOptions = services.ExecutePreparedOptions<RamshaAccountContractsOptions>();

        var accountServiceImplementation = contractsOptions.ReplacedAccountServiceType is not null ? contractsOptions.ReplacedAccountServiceType : typeof(RamshaAccountService<,,,,,,,,>).MakeGenericType(
            entityTypes.UserType,
            entityTypes.RoleType,
            entityTypes.KeyType,
            entityTypes.UserRoleType,
            entityTypes.RoleClaimType,
            entityTypes.UserClaimType,
            entityTypes.UserLoginType,
            entityTypes.UserTokenType,
           contractsOptions.GetReplacedDtoOrBase<RamshaRegisterDto>()

        );
        var accountServiceInterface = typeof(IRamshaAccountService<>)
        .MakeGenericType(
           contractsOptions.GetReplacedDtoOrBase<RamshaRegisterDto>()
            );

        services.AddRamshaService(accountServiceImplementation, accountServiceInterface);


        return services;
    }
}
