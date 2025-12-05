namespace Ramsha.Settings;

public interface ISettingValueResolverManager
{
    IReadOnlyList<ISettingValueResolver> Resolvers { get; }
}
