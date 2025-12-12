namespace Ramsha.Cli.Core;

public class DotnetTemplateRegistry : BaseTemplateRegistry<DotnetTemplateInfo>
{
    public DotnetTemplateRegistry()
    {
        RegisterTemplates([
            new(
                DotnetTemplates.WebApi.Name,
                DotnetTemplates.WebApi.DotnetShortName,
                DotnetTemplates.WebApi.PackageName
                ),
            new(
                DotnetTemplates.CleanWebApi.Name,
                DotnetTemplates.CleanWebApi.DotnetShortName,
                DotnetTemplates.CleanWebApi.PackageName
                ),
            ]);
    }
}

