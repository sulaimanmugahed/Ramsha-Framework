namespace Ramsha.Settings;

public class SettingsModule : RamshaModule
{
    public override void BuildServices(BuildServicesContext context)
    {
        base.BuildServices(context);
        context.Services.AddSettingsServices();
    }
}
