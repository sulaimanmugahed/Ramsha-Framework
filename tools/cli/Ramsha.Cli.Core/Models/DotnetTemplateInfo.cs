namespace Ramsha.Cli.Core;

public class DotnetTemplateInfo : TemplateInfo
{
    public string ShortName { get; }
    public string? PackageName { get; }

    public DotnetTemplateInfo(
        string name,
        string shortName,
        string? packageName = null) : base(name)

    {
        ShortName = shortName;
        PackageName = packageName;
    }
}
