using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Core.Options;

public class RamshaIdentityTypesOptions
{
    public Type UserType { get; set; }
    public Type RoleType { get; set; }
    public Type KeyType { get; set; }
    public Type UserRoleType { get; set; }
    public Type RoleClaimType { get; set; }
    public Type UserClaimType { get; set; }
    public Type UserLoginType { get; set; }
    public Type UserTokenType { get; set; }

    public Type[] GetTypesParams()
    {
        return [UserType, RoleType, KeyType, UserRoleType, RoleClaimType, UserClaimType, UserLoginType, UserTokenType];
    }



}
