using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Authorization;

namespace DemoApp.Permissions;

public class ProductPermissions : IPermissionDefinitionProvider
{
    public void Define(IPermissionDefinitionContext context)
    {
        context.Group("products", g =>
        {
            g.Permission("manage").Children(c =>
            {
                c.Add("create");
                c.Add("update");
                c.Add("delete");
            });
        });
    }
}
