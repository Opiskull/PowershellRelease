using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PowershellHelper.Services
{
    public class AssemblyVersionFileHelper
    {
        public const string AssemblyVersionPattern = "^\\[assembly: AssemblyVersion\\(\"(.*)\"\\)\\]";
        public const string AssemblyFileVersionPattern = "^\\[assembly: AssemblyFileVersion\\(\"(.*)\"\\)\\]";
        public const string AssemblyInformationalVersionPattern =
            "^\\[assembly: AssemblyInformationalVersion\\(\"(.*)\"\\)\\]";
        
        private string ReadContentRegex(string assemblyContent, string regex)
        {
            var matches = Regex.Matches(assemblyContent, regex, RegexOptions.Multiline);
            if (matches.Count == 0)
            {
                return string.Empty;
            }
            var match = matches[0];
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public string ReadAssemblyVersion(string assemblyContent)
        {
            return ReadContentRegex(assemblyContent,AssemblyVersionPattern);
        }

        public string ReadAssemblyFileVersion(string assemblyContent)
        {
            return ReadContentRegex(assemblyContent, AssemblyFileVersionPattern);
        }

        public string ReadAssemblyInformationalVersion(string assemblyContent)
        {
            return ReadContentRegex(assemblyContent, AssemblyInformationalVersionPattern);
        }

        private string WriteContentRegex(string assemblyContent, string regex, string newContent)
        {
            return Regex.Replace(assemblyContent, regex, newContent, RegexOptions.Multiline);
        }

        public string WriteAssemblyVersion(string assemblyContent, string version)
        {
            var newAssemblyVersion = $"[assembly: AssemblyVersion(\"{version}\")]";
            return WriteContentRegex(assemblyContent,AssemblyVersionPattern, newAssemblyVersion);
        }

        public string WriteAssemblyFileVersion(string assemblyContent, string version)
        {
            var newAssemblyFileVersion = $"[assembly: AssemblyFileVersion(\"{version}\")]";
            return WriteContentRegex(assemblyContent, AssemblyFileVersionPattern, newAssemblyFileVersion);
        }

        public string WriteAssemblyInformationalVersion(string assemblyContent, string version)
        {
            var newAssemblyInformationalVersion = $"[assembly: AssemblyInformationalVersion(\"{version}\")]";
            return WriteContentRegex(assemblyContent, AssemblyInformationalVersionPattern, newAssemblyInformationalVersion);
        }
        
        public string ReadAssemblyVersionFromPath(string assemblyPath)
        {
            var content = File.ReadAllText(assemblyPath);
            return ReadAssemblyVersion(content);
        }

        public void WriteAssemblyVersionToPath(string assemblyPath, string version)
        {
            var content = File.ReadAllText(assemblyPath);
            content = WriteAssemblyVersion(content, version);
            content = WriteAssemblyFileVersion(content, version);
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