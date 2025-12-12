using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public class EmbeddedTemplateRegistry : BaseTemplateRegistry<EmbeddedTemplateInfo>
{
    public EmbeddedTemplateRegistry()
    {

        RegisterTemplates([
               new EmbeddedTemplateInfo(EmbeddedTemplates.Controller.Name,
            EmbeddedTemplates.Controller.ResourcePath,
             EmbeddedTemplates.Controller.OutputFileExtension)
             .WithSuffix(EmbeddedTemplates.Controller.Suffix,EmbeddedTemplates.Controller.CommonSuffix)
            ]);
    }


}

