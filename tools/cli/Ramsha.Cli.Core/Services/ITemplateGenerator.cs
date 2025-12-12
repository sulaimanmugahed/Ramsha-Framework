

namespace Ramsha.Cli.Core;

public interface ITemplateGenerator<TModel>
       where TModel : ITemplateModel
{
    Task GenerateAsync(TModel model);
}

public interface ITemplateGenerator
{
    Task GenerateAsync(TemplateModel model);
}