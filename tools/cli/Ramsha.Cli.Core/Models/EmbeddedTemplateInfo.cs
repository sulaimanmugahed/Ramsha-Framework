using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public class EmbeddedTemplateInfo : TemplateInfo
{
    public EmbeddedTemplateInfo(string name, string resourcePath, string outputFileExtension, string? outputFilename = null) : base(name)
    {
        ResourcePath = resourcePath;
        OutputFileExtension = outputFileExtension;
        OutputFilename = outputFilename;
    }

    public string ResourcePath { get; set; }
    public string OutputFileExtension { get; set; }
    public string? OutputFilename { get; set; }
    public string? Suffix { get; set; }
    public List<string> CommonSuffix { get; set; }

    public EmbeddedTemplateInfo WithSuffix(string suffix, List<string> commonSuffix)
    {
        Suffix = suffix;
        CommonSuffix = commonSuffix;
        return this;
    }

    public string GetOutputFileName(string fileName)
    {
        if (Suffix is null)
        {
            return fileName;
        }

        if (CommonSuffix.Any(fileName.EndsWith))
        {
            return fileName;
        }

        return fileName + Suffix;
    }

    public string GetOutputFileNameWithExtension(string fileName)
    => $"{GetOutputFileName(fileName)}.{OutputFileExtension}";
    public string GetOutputFileNameWithExtension()
   => $"{OutputFilename}.{OutputFileExtension}";

    public string GetOutputFilePath(string fileName, string outputDirectory)
    => Path.Combine(outputDirectory, GetOutputFileNameWithExtension(fileName));

    public string GetOutputFilePath(string outputDirectory)
  => Path.Combine(outputDirectory, GetOutputFileNameWithExtension());
}
