using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Security.Users;

namespace Ramsha.SettingsManagement.Domain;

public class UserSettingManagerResolver(
    ISettingManagerStore store,
    ICurrentUser currentUser)
: SettingManagerResolver("U", store)
{
    protected override string GetNormalizedProviderKey(string providerKey)
    {
        return currentUser.Id;
    }
}
