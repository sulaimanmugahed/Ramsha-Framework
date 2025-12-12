
using Ramsha.Cli.Core;

namespace Ramsha.Cli.Infrastructure;


public class EmbeddedTemplateGenerator<TModel>(
  ITemplateRenderer renderer,
  ITemplateRegistry<EmbeddedTemplateInfo> registry)
  : BaseTemplateGenerator<TModel, EmbeddedTemplateInfo>(registry), IEmbeddedTemplateGenerator<TModel>
  where TModel : ITemplateModel<TModel>
{

    public override async Task GenerateAsync(TModel model)
    {
        var template = GetTemplateInfo(model.TemplateName);

        var outputDirectory = GetOutputDirectory(model);

        Directory.CreateDirectory(outputDirectory);

        if (!model.ContainsParam(CommonParams.Namespace) || model.Get<string>(CommonParams.Namespace) is null)
        {
            model.WithParam(CommonParams.Namespace, ResolveDefaultNamespace(outputDirectory));
        }

        var templateContent = await EmbeddedResourceHelper.ReadAsStringAsync(template.ResourcePath);

        var fullOutputFilePathWithExtension = !model.ContainsParam(CommonParams.Name) ? template.GetOutputFilePath(outputDirectory) : template.GetOutputFilePath(model.Get<string>(CommonParams.Name)!, outputDirectory);

        var result = renderer.RenderTemplate(templateContent, model.GetParameters());
        await File.WriteAllTextAsync(fullOutputFilePathWithExtension, result);
    }

    private string GetOutputDirectory(TModel model)
    {
        return model.ContainsParam(CommonParams.Output) ? model.Get<string>(CommonParams.Output)! : ".";
    }
}

public class EmbeddedTemplateGenerator(
   ITemplateRenderer renderer,
   ITemplateRegistry<EmbeddedTemplateInfo> registry)
   : BaseTemplateGenerator<EmbeddedTemplateInfo>(registry), IEmbeddedTemplateGenerator

{

    public override async Task GenerateAsync(TemplateModel model)
    {
        var template = GetTemplateInfo(model.TemplateName);

        model.WithName(template.GetOutputFileName(model.Get<string>(CommonParams.Name)!));

        var outputDirectory = GetOutputDirectory(model);

        Directory.CreateDirectory(outputDirectory);

        if (!model.ContainsParam(CommonParams.Namespace) || string.IsNullOrEmpty(model.Get<string>(CommonParams.Namespace)))
        {
            model.WithParam(CommonParams.Namespace, ResolveDefaultNamespace(outputDirectory));
        }

        var templateContent = await EmbeddedResourceHelper.ReadAsStringAsync(template.ResourcePath);

        var fullOutputFilePathWithExtension = !model.ContainsParam(CommonParams.Name) ? template.GetOutputFilePath(outputDirectory) : template.GetOutputFilePath(string.IsNullOrEmpty(model.Get<string>(CommonParams.Name)) ? "NoName" : model.Get<string>(CommonParams.Name)!, outputDirectory);

        var result = renderer.RenderTemplate(templateContent, model.GetParameters());
        await File.WriteAllTextAsync(fullOutputFilePathWithExtension, result);
    }

    private string GetOutputDirectory(TemplateModel model)
    {
        return model.ContainsParam(CommonParams.Output) ? model.Get<string>(CommonParams.Output)! : ".";
    }
}

