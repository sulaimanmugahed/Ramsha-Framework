namespace Ramsha.Authorization;

public class RamshaPermissionOptions
{
    public ITypeList<IPermissionProviderResolver> PermissionResolvers { get; }
    public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }


    public RamshaPermissionOptions()
    {
        PermissionResolvers = new TypeList<IPermissionProviderResolver>();
        DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();

    }
}


