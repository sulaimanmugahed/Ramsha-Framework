using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Settings;

public class RamshaSettingsOptions
{
    public ITypeList<ISettingDefinitionProvider> DefinitionProviders { get; }
    public ITypeList<ISettingValueResolver> ValueResolvers { get; }


    public RamshaSettingsOptions()
    {
        DefinitionProviders = new TypeList<ISettingDefinitionProvider>();
        ValueResolvers = new TypeList<ISettingValueResolver>();

    }
}
