using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Core;

namespace Ramsha.Identity.Domain;

public class IdentityDomainModule : RamshaModule
{

    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityCoreModule>();

        moduleBuilder.PreConfigure<RamshaIdentityOptions>(options => { });
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);

        var options = context.Services.ExecutePreConfigured<RamshaIdentityOptions>();


        var addIdentityCoreMethod = typeof(IdentityServiceCollectionExtensions)
            .GetMethods()
            .First(m => m.Name == nameof(IdentityServiceCollectionExtensions.AddIdentityCore)
                     && m.IsGenericMethodDefinition
                     && m.GetParameters().Length == 2);

        var genericAddIdentityCore = addIdentityCoreMethod.MakeGenericMethod(options.UserType);

        var builder = (IdentityBuilder)genericAddIdentityCore.Invoke(
            null,
            new object[]
            {
                    context.Services,
                    options.IdentityOptionsAction ?? new Action<IdentityOptions>(opt =>
                    {
                        opt.Password.RequireDigit = false;
                        opt.Password.RequireNonAlphanumeric = false;
                        opt.Password.RequireUppercase = false;
                        opt.Password.RequiredLength = 4;
                    })
            })!;

        var addRolesGeneric = typeof(IdentityBuilder)
      .GetMethod(nameof(IdentityBuilder.AddRoles))
      ?.MakeGenericMethod(options.RoleType);

        addRolesGeneric?.Invoke(builder, null);


        var factoryType = typeof(RamshaUserClaimsPrincipalFactory<,,,,,,,>).MakeGenericType(options.UserType, options.RoleType, options.KeyType, options.UserRoleType, options.RoleClaimType, options.UserClaimType, options.UserLoginType, options.UserTokenType);
        var addClaimsFactoryGeneric = typeof(IdentityBuilder)
            .GetMethods()
            .First(m => m.Name == nameof(IdentityBuilder.AddClaimsPrincipalFactory)
                     && m.IsGenericMethodDefinition
                     && m.GetParameters().Length == 0)
            .MakeGenericMethod(factoryType);

        addClaimsFactoryGeneric.Invoke(builder, null);

        foreach (var configureAction in options.ConfigureIdentityActions)
        {
            configureAction(builder);
        }

        context.Services.AddObjectAccessor(builder);
    }
}
