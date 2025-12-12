namespace Ramsha.Cli.Core;

public interface ITemplateRenderer
{
    string? RenderTemplate(string templateName, object model);
}
