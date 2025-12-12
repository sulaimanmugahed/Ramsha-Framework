using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public static class EmbeddedTemplates
{
    public const string TemplateFolderPath = "Templates";


    public static class Command
    {
        public const string Name = "Command";
        public const string ResourcePath = $"{TemplateFolderPath}.Command.scriban";
        public const string OutputFileExtension = "cs";
        public const string Suffix = "Command";
    }
    public static class Query
    {
        public const string Name = "Query";
        public const string ResourcePath = $"{TemplateFolderPath}.Query.scriban";
        public const string OutputFileExtension = "cs";
        public const string Suffix = "Query";
    }

    public static class Controller
    {
        public const string Name = "Controller";
        public const string ResourcePath = $"{TemplateFolderPath}.Controller.scriban";
        public const string OutputFileExtension = "cs";
        public const string Suffix = "Controller";
        public static List<string> CommonSuffix = ["Controller", "Endpoint", "Endpoints"];

    }


    public static class DomainManager
    {
        public const string Name = "DomainManager";
        public const string ResourcePath = $"{TemplateFolderPath}.DomainManager.scriban";
        public const string OutputFileExtension = "cs";
        public const string Suffix = "Manager";
        public static List<string> CommonSuffix = ["Manager", "Service"];

    }

    public static class Service
    {
        public const string Name = "Service";
        public const string ResourcePath = $"{TemplateFolderPath}.Service.scriban";
        public const string OutputFileExtension = "cs";
        public const string Suffix = "Service";
    }

    public static class Entity
    {
        public const string Name = "Entity";
        public const string ResourcePath = $"{TemplateFolderPath}.Entity.scriban";
        public const string OutputFileExtension = "cs";
    }



}
