namespace Ramsha.Cli.Core;

public interface ITemplateRegistry<TTemplateInfo>
    where TTemplateInfo : TemplateInfo
{
    TTemplateInfo? GetTemplate(string name);
    List<TTemplateInfo> GetTemplateList();

}

public interface ITemplateRegistry
{
    TemplateInfo? GetTemplate(string name);
}

