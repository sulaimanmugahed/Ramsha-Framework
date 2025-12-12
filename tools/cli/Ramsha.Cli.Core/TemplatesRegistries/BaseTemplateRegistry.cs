namespace Ramsha.Cli.Core;

public class BaseTemplateRegistry<TTemplateInfo> : ITemplateRegistry<TTemplateInfo>
      where TTemplateInfo : TemplateInfo
{
    private readonly Dictionary<string, TTemplateInfo> _templates;

    public BaseTemplateRegistry()
    {
        _templates ??= [];
    }
    public TTemplateInfo? GetTemplate(string name)
    => _templates.TryGetValue(name, out var template) ? template : default;

    public List<TTemplateInfo> GetTemplateList()
    => _templates.Values.ToList();

    protected void RegisterTemplates(IEnumerable<TTemplateInfo> templates)
    {
        foreach (var template in templates)
        {
            var name = template?.Name
                       ?? throw new InvalidOperationException("Template must implement TemplateInfo");

            _templates[name] = template;
        }
    }
}

