using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public class ControllerModel(string name, string directory, string? @namespace)
       : TemplateModel<ControllerModel>(EmbeddedTemplates.Controller.Name), INamespaceModel
{
    public string? Namespace { get; set; } = @namespace;

    public string Name { get; set; } = name;
    public string OutputDirectory { get; set; } = directory;
}
