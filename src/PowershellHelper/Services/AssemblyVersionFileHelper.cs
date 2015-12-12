using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PowershellHelper.Services
{
    public class AssemblyVersionFileHelper
    {
        public const string AssemblyVersionPattern = "^\\[assembly: AssemblyVersion\\(\"(.*)\"\\)\\]";
        public const string AssemblyFileVersionPattern = "^\\[assembly: AssemblyFileVersion\\(\"(.*)\"\\)\\]";

        public string ReadAssemblyVersion(string assemblyContent)
        {
            var matches = Regex.Matches(assemblyContent, AssemblyVersionPattern);
            if (matches.Count == 0)
            {
                return string.Empty;
            }
            var match = matches[0];
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public string ReadAssemblyVersionFromFile(string assemblyPath)
        {
            var content = File.ReadAllText(assemblyPath);
            return ReadAssemblyVersion(content);
        }

        public string WriteAssemblyVersion(string assemblyContent, string version)
        {
            var newAssemblyVersion = $"[assembly: AssemblyVersion(\"{version}\")]";
            var newFileVersion = $"[assembly: AssemblyFileVersion(\"{version}\")]";
            assemblyContent = Regex.Replace(assemblyContent, AssemblyVersionPattern, newAssemblyVersion);
            assemblyContent = Regex.Replace(assemblyContent, AssemblyFileVersionPattern, newFileVersion);
            return assemblyContent;
        }

        public void WriteAssemblyVersionToFile(string assemblyPath, string version)
        {
            var content = File.ReadAllText(assemblyPath);
            content = WriteAssemblyVersion(content, version);
            File.WriteAllText(assemblyPath, content);
        }

        public string IncrementAssemblyVersionRevision(string version)
        {
            var ver = Version.Parse(version);
            return new Version(ver.Major, ver.Minor, ver.Build, ver.Revision + 1).ToString();
        }

        public string IncrementAssemblyVersionBuild(string version)
        {
            var ver = Version.Parse(version);
            return new Version(ver.Major, ver.Minor, ver.Build + 1, 0).ToString();
        }
    }
}