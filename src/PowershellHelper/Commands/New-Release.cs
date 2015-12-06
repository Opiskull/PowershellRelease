using System.Management.Automation;
using LibGit2Sharp;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.New, "Release",
        DefaultParameterSetName = "VersionCommit")]
    public class NewRelease : PSCmdlet
    {
        [Parameter(
            ParameterSetName = "VersionCommit",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string AssemblyFilePath { get; set; }

        [Parameter(
            ParameterSetName = "VersionCommit",
            Mandatory = true,
            Position = 1)]
        [ValidateNotNullOrEmpty]
        public string RepositoryPath { get; set; }

        [Parameter(
            ParameterSetName = "VersionCommit",
            Mandatory = true)]
        public string UserName { get; set; }

        [Parameter(
            ParameterSetName = "VersionCommit",
            Mandatory = true)]
        public string UserEmail { get; set; }

        [Parameter(
            ParameterSetName = "VersionCommit")]
        public string UserPassword { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(UserPassword))
            {
                WriteWarning("The UserPassword is empty and DefaultCredentials will be used");
            }

            var gitHelper = new GitRepositoryHelper(RepositoryPath,UserName,UserPassword);
            var assemblyHelper = new AssemblyVersionFileHelper();

            // Assembly
            var readVersion = assemblyHelper.ReadAssemblyVersionFromFile(AssemblyFilePath);
            WriteVerbose($"Found Version {readVersion}");
            var newVersion = assemblyHelper.IncrementAssemblyVersionRevision(readVersion);
            WriteVerbose($"Increment Version to {newVersion}");
            assemblyHelper.WriteAssemblyVersionToFile(AssemblyFilePath, newVersion);

            // Git
            WriteVerbose($"Commit File {AssemblyFilePath}");
            gitHelper.CommitFile(AssemblyFilePath, UserName, UserEmail, $"chore(release): {newVersion}");
            WriteVerbose($"Create Tag {newVersion}");
            gitHelper.CreateTag(newVersion);

            // Push
            WriteVerbose($"Push Tag {newVersion}");
            gitHelper.PushTag(newVersion);
            var branch = gitHelper.Repository.Head.Name;
            WriteVerbose($"Push Branch {branch} to Remote origin");
            gitHelper.PushHeadBranch();
        }
    }
}