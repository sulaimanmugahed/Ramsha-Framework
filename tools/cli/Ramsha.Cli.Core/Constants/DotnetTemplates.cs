using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public static class DotnetTemplates
{
    public const string RamshaDotnetPackage = "Ramsha.Templates";

    public static class WebApi
    {
        public const string Name = "WebApi";
        public const string DotnetShortName = "ramsha-api";
        public const string PackageName = RamshaDotnetPackage;
    }
    public static class CleanWebApi
    {
        public const string Name = "CleanWebApi";
        public const string DotnetShortName = "ramsha-c-api";
        public const string PackageName = RamshaDotnetPackage;
    }
}
