using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public class DotnetTemplateModel(string templateName)
    : TemplateModel<DotnetTemplateModel>(templateName)
{
    public DotnetTemplateModel WithForce(bool value = false)
    {
        return WithParam(DotnetTemplateParams.Force, value);
    }
}


public class TemplateModel<T> : ITemplateModel<T> where T : TemplateModel<T>
{
    private readonly Dictionary<string, object> _parameters = new();
    public TemplateModel(string templateName)
    {
        TemplateName = templateName;
    }
    public string TemplateName { get; init; }

    public Dictionary<string, object> GetParameters()
    => new(_parameters);

    public object? this[string key] => _parameters.TryGetValue(key, out var val) ? val : null;

    public bool ContainsParam(string key) => _parameters.ContainsKey(key);
    public IEnumerable<string> Keys => _parameters.Keys;

    public T WithParam(string name, object? value, bool required = false)
    {
        if (required && value is null) throw new ArgumentNullException(nameof(value));

        else if (!required && value is null)
            return (T)this;

        _parameters[name] = value!;
        return (T)this;
    }

    public T WithStringParam(string name, string? value, bool required = false)
    {
        if (required && string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        return WithParam(name, value, required);
    }

    public T WithNamespace(string? @namespace, bool required = false)
    {
        return WithStringParam(CommonParams.Namespace, @namespace, required);
    }

    public T WithName(string name, bool required = false)
    {
        return WithStringParam(CommonParams.Name, name, required);
    }

    public T WithOutput(string output, bool required = false)
    {
        return WithStringParam(CommonParams.Output, output, required);
    }

    public T WithCSharpProperties(List<CSharpProperty> properties)
    {
        return WithParam(CommonParams.CSharpProperties, properties);
    }

}
public class TemplateModel : ITemplateModel<TemplateModel>
{
    private readonly Dictionary<string, object> _parameters = new();
    public TemplateModel(string templateName)
    {
        TemplateName = templateName;
    }

    public object? this[string key] => _parameters.TryGetValue(key, out var val) ? val : null;

    public string TemplateName { get; init; }

    public IEnumerable<string> Keys => _parameters.Keys;

    public bool ContainsParam(string key) => _parameters.ContainsKey(key);


    public Dictionary<string, object> GetParameters()
   => new(_parameters);

    public TemplateModel WithParam(string name, object? value, bool required = false)
    {
        if (required && value is null) throw new ArgumentNullException(nameof(value));

        else if (!required && value is null)
            return this;

        _parameters[name] = value!;
        return this;
    }

    public TemplateModel WithStringParam(string name, string? value, bool required = false)
    {
        if (required && string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
        return WithParam(name, value, required);
    }

    public TemplateModel WithNamespace(string? @namespace, bool required = false)
    {
        return WithStringParam(CommonParams.Namespace, @namespace, required);
    }

    public TemplateModel WithName(string? name, bool required = false)
    {
        return WithStringParam(CommonParams.Name, name, required);
    }

    public TemplateModel WithOutput(string? output, bool required = false)
    {
        return WithStringParam(CommonParams.Output, output, required);
    }

    public TemplateModel WithCSharpProperties(List<CSharpProperty> properties)
    {
        return WithParam(CommonParams.CSharpProperties, properties);
    }



}



public interface IHasDotnetAdditionalParameters
{
    Dictionary<string, string> DotnetAdditionalParameters { get; }
}
public interface IHasName
{
    string Name { get; set; }
}
public interface IHasOutDirectory
{
    string OutputDirectory { get; set; }
}
public interface IHasParameter
{
    IEnumerable<string> Keys { get; }
    object? this[string key] { get; }
    bool ContainsParam(string key);
    Dictionary<string, object> GetParameters();
}
public interface IHasParameter<T> : IHasParameter where T : IHasParameter<T>
{
    T WithParam(string name, object value, bool required = false);
}

public interface ITemplateModel
{
    string TemplateName { get; init; }
}

public interface ITemplateModel<T> : ITemplateModel, IHasParameter<T> where T : ITemplateModel<T>
{

}

public interface INamespaceModel : IHasName, IHasOutDirectory
{
    string? Namespace { get; set; }
}