namespace Ramsha.Authorization;

public interface IPermissionDefinitionContext
{
    void Group(string name, Action<GroupBuilder> configure);
}


