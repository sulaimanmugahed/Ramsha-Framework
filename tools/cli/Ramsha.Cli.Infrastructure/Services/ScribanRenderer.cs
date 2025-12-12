using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Cli.Core;

namespace Ramsha.Cli.Infrastructure;

public class ScribanRenderer : ITemplateRenderer
{
    public string? RenderTemplate(string templateContent, object model)
    {
        return Scriban.Template.Parse(templateContent).Render(model);
    }
}
