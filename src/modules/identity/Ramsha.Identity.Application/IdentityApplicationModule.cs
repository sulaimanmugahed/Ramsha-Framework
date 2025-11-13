using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Contracts;
using Ramsha.Identity.Core.Options;
using Ramsha.Identity.Domain;
using Ramsha.LocalMessaging.Abstractions;

namespace Ramsha.Identity.Application;

public class IdentityApplicationModule : RamshaModule
{
    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder
        .DependsOn<IdentityContractsModule>()
        .DependsOn<IdentityDomainModule>();


        moduleBuilder.OnCreatingConfigure<LocalMessagingOptions>(messageOptions =>
        {
            messageOptions.AddCommandHandler(typeof(CreateRoleCommandHandler<,,>));
        });
        ;





    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);
        var entityTypes = context.Services.ExecutePreConfigured<RamshaIdentityTypesOptions>();
        var contractsOptions = context.Services.ExecutePreConfigured<RamshaIdentityContractsOptions>();

        var userServiceImplementation = contractsOptions.ReplacedUserServiceType is not null ? contractsOptions.ReplacedUserServiceType : typeof(RamshaIdentityUserService<,,,,,,,,,>).MakeGenericType(
            entityTypes.UserType,
            entityTypes.RoleType,
            entityTypes.KeyType,
            entityTypes.UserRoleType,
            entityTypes.RoleClaimType,
            entityTypes.UserClaimType,
            entityTypes.UserLoginType,
            entityTypes.UserTokenType,
            contractsOptions.CreateUserDtoType,
            contractsOptions.CreateUserDtoType

        );
        var userServiceInterface = typeof(IRamshaIdentityUserService<,,>)
        .MakeGenericType(
            contractsOptions.CreateUserDtoType,
            contractsOptions.CreateUserDtoType,
            entityTypes.KeyType
            );

        context.Services.AddTransient(userServiceInterface, userServiceImplementation);
    }
}
