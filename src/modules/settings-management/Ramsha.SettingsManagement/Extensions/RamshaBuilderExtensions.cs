using Ramsha.SettingsManagement;

namespace Ramsha;

public static class RamshaBuilderExtensions
{
    public static RamshaBuilder AddSettingsManagementModule(this RamshaBuilder ramsha)
    {
        ramsha.AddModule<SettingsManagementModule>();
        return ramsha;
    }
}
