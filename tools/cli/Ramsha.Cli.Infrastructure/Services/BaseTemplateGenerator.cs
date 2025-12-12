using Ramsha.Cli.Core;

namespace Ramsha.Cli.Infrastructure;

public abstract class BaseTemplateGenerator<TModel, TTemplateInfo>(ITemplateRegistry<TTemplateInfo> registry) : ITemplateGenerator<TModel>
 where TModel : ITemplateModel
 where TTemplateInfo : TemplateInfo
{
    public abstract Task GenerateAsync(TModel model);

    protected virtual TTemplateInfo GetTemplateInfo(string templateName)
    {
        var template = registry.GetTemplate(templateName);
        if (template is null)
        {
            throw new Exception($"Template '{templateName}' not found in registry");
        }
        return template;
    }



    protected virtual string ResolveDefaultNamespace(string outputPath)
    {

        var projectInfo = ProjectHelpers.FindNearestProject(outputPath);
        var relativePath = Path.GetRelativePath(
            projectInfo.RootDirectory,
            Path.GetFullPath(outputPath));

        return $"{projectInfo.ProjectName}.{relativePath}"
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace("..", ".")
            .Trim('.');
    }

}

public abstract class BaseTemplateGenerator<TTemplateInfo>(ITemplateRegistry<TTemplateInfo> registry) : ITemplateGenerator
   where TTemplateInfo : TemplateInfo
{
    public abstract Task GenerateAsync(TemplateModel model);

    protected virtual TTemplateInfo GetTemplateInfo(string templateName)
    {
        var template = registry.GetTemplate(templateName);
        if (template is null)
        {
            throw new Exception($"Template '{templateName}' not found in registry");
        }
        return template;
    }



    protected virtual string ResolveDefaultNamespace(string outputPath)
    {

        var projectInfo = ProjectHelpers.FindNearestProject(outputPath);
        var relativePath = Path.GetRelativePath(
            projectInfo.RootDirectory,
            Path.GetFullPath(outputPath));

        return $"{projectInfo.ProjectName}.{relativePath}"
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace("..", ".")
            .Trim('.');
    }

}

