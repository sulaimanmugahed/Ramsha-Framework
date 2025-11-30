namespace Ramsha.Authorization;

public class RamshaPermissionOptions
{
    public ITypeList<IPermissionProviderResolver> PermissionResolvers { get; }

    public RamshaPermissionOptions()
    {
        PermissionResolvers = new TypeList<IPermissionProviderResolver>();
    }
}


