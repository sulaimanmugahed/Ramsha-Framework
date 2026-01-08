using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Settings;

namespace SimpleAppDemo;

public class SettingDef : ISettingDefinitionProvider
{
    public void Define(ISettingDefinitionContext context)
    {
        context.Group("MyGroup", g =>
        {
            g.Setting("TestOption", "default value !!").Providers(["M"]);
        });
    }
}
