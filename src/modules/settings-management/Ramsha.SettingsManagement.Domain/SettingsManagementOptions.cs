using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.SettingsManagement.Domain;

public class SettingsManagementOptions
{
    public ITypeList<ISettingManagerResolver> ManagerResolvers { get; }


    public SettingsManagementOptions()
    {
        ManagerResolvers = new TypeList<ISettingManagerResolver>();
    }
}
