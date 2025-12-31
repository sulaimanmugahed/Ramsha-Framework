using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Shared;

public static class RamshaIdentityErrorsCodes
{
    public const string IDENTITY_PREFIX = "identity:";
    public const string IDENTITY_DEFAULT = IDENTITY_PREFIX + "default";
    public const string GenerateUsernameErrorCode = IDENTITY_PREFIX + "generateUsername";

}
