using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ramsha.Identity.Domain;

namespace Ramsha.Identity.Persistence.Extensions;

public static class RamshaIdentityOptionsExtensions
{
    public static RamshaIdentityOptions AddEntityFrameworkCore(this RamshaIdentityOptions options)
    {
        options.ConfigureIdentity(builder =>
           {
               builder
               .AddRamshaIdentityEntityFrameworkCore(
                   options.UserType,
                    options.RoleType,
                     options.UserRoleType,
                      options.RoleClaimType,
                       options.UserClaimType,
                        options.UserLoginType,
                         options.UserTokenType);
           });
        return options;
    }
}
