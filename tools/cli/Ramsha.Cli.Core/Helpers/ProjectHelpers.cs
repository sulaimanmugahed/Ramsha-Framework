using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Cli.Core;

public class ProjectHelpers
{
    public static ProjectInfo FindNearestProject(string startDirectory)
    {
        var directory = new DirectoryInfo(startDirectory);

        while (directory != null)
        {
            var projectFiles = directory.GetFiles("*.csproj");
            if (projectFiles.Length > 0)
            {
                var projectFile = projectFiles[0];
                return new ProjectInfo(
                    ProjectPath: projectFile.FullName,
                    ProjectName: Path.GetFileNameWithoutExtension(projectFile.Name),
                    RootDirectory: directory.FullName
                );
            }

            directory = directory.Parent;
        }


        var dirName = new DirectoryInfo(startDirectory).Name;
        return new ProjectInfo(
            ProjectPath: string.Empty,
            ProjectName: dirName,
            RootDirectory: startDirectory
        );
    }



}
