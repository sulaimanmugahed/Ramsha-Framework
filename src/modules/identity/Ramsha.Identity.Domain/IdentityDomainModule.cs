using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Core;
using Ramsha.Identity.Core.Options;

namespace Ramsha.Identity.Domain;

public class IdentityDomainModule : RamshaModule
{

    public override void OnCreating(ModuleBuilder moduleBuilder)
    {
        base.OnCreating(moduleBuilder);
        moduleBuilder.DependsOn<IdentityCoreModule>();

        moduleBuilder.OnCreatingConfigure<RamshaIdentityTypesOptions>(options =>
        {
            options.KeyType ??= typeof(Guid);
            options.UserType ??= typeof(RamshaIdentityUser);
            options.RoleType ??= typeof(RamshaIdentityRole);
            options.UserClaimType ??= typeof(RamshaIdentityUserClaim<Guid>);
            options.UserLoginType ??= typeof(RamshaIdentityUserLogin<Guid>);
            options.UserTokenType ??= typeof(RamshaIdentityUserToken<Guid>);
            options.RoleClaimType ??= typeof(RamshaIdentityRoleClaim<Guid>);
            options.UserRoleType ??= typeof(RamshaIdentityUserRole<Guid>);
        });

        moduleBuilder.OnCreatingConfigure<RamshaIdentityOptions>(options => { });
    }

    public override void OnConfiguring(ConfigureContext context)
    {
        base.OnConfiguring(context);

        var options = context.Services.ExecutePreConfigured<RamshaIdentityOptions>();
        var typesOptions = context.Services.ExecutePreConfigured<RamshaIdentityTypesOptions>();


        var addIdentityCoreMethod = typeof(IdentityServiceCollectionExtensions)
            .GetMethods()
            .First(m => m.Name == nameof(IdentityServiceCollectionExtensions.AddIdentityCore)
                     && m.IsGenericMethodDefinition
                     && m.GetParameters().Length == 2);

        var genericAddIdentityCore = addIdentityCoreMethod.MakeGenericMethod(typesOptions.UserType);

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
      ?.MakeGenericMethod(typesOptions.RoleType);

        addRolesGeneric?.Invoke(builder, null);


        var factoryType = typeof(RamshaUserClaimsPrincipalFactory<,,,,,,,>)
        .MakeGenericType(typesOptions.UserType, typesOptions.RoleType, typesOptions.KeyType, typesOptions.UserRoleType, typesOptions.RoleClaimType, typesOptions.UserClaimType, typesOptions.UserLoginType, typesOptions.UserTokenType);
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
        
         context.Services.AddRamshaIdentityDomainServices();

        context.Services.AddObjectAccessor(builder);
    }
}
