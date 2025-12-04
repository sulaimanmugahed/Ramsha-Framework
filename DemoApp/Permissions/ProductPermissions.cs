using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Authorization;

namespace DemoApp.Permissions;

public static class ProductPermissions
{
    public const string GroupName = "products";

    public static class Manage
    {
        public const string Default = $"{GroupName}:manage";

        public const string Create = $"{GroupName}:manage:create";
        public const string Update = $"{GroupName}:manage:update";
        public const string Delete = $"{GroupName}:manage:delete";
    }
}

public class ProductPermissionsDefinition : IPermissionDefinitionProvider
{
    public void Define(IPermissionDefinitionContext context)
    {
        context.Group(ProductPermissions.GroupName, g =>
        {
            g.Permission(ProductPermissions.Manage.Default).Children(c =>
            {
                c.Add(ProductPermissions.Manage.Create);
                c.Add(ProductPermissions.Manage.Update);
                c.Add(ProductPermissions.Manage.Delete);
            });
        });
    }
}
