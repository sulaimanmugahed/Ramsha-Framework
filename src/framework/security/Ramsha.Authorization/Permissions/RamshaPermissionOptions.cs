namespace Ramsha.Authorization;

public class RamshaPermissionOptions
{
    public ITypeList<IPermissionValueResolver> ValueResolvers { get; }
    public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }


    public RamshaPermissionOptions()
    {
        ValueResolvers = new TypeList<IPermissionValueResolver>();
        DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();

    }
}


