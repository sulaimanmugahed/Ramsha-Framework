namespace Ramsha.Cli.Core;

public interface IEmbeddedTemplateGenerator<TModel> : ITemplateGenerator<TModel>
    where TModel : ITemplateModel
{
}

public interface IEmbeddedTemplateGenerator : ITemplateGenerator
{
}
