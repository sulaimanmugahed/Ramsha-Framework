using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;



public record CSharpProperty(string Type, string Name);

public static class ListExtensions
{

    public static T? Get<T, TModel>(this IHasParameter<TModel> model, string key)

    where TModel : IHasParameter<TModel>
    {
        return model[key] is T value ? value : default;
    }
    public static List<CSharpProperty> ParseProps(this string[]? properties)
    {
        return properties?.Select(p => p.Split(':') switch
        {
            [var name, "i"] => new CSharpProperty("int", name),
            [var name, "d"] => new CSharpProperty("double", name),
            [var name, "dt"] => new CSharpProperty("DateTime", name),
            [var name, "b"] => new CSharpProperty("bool", name),
            [var name] => new CSharpProperty("string", name),
            _ => throw new ArgumentException($"Invalid property format: {p}")
        }).ToList() ?? new List<CSharpProperty>();
    }


}
