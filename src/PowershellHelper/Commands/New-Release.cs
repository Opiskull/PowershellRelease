using System.Management.Automation;
using LibGit2Sharp;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.New, "Release",
        DefaultParameterSetName = "NewRelease")]
    public class NewRelease : PSCmdlet
    {
        [Parameter(
            ParameterSetName = "NewRelease",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string RepositoryPath { get; set; }

        [Parameter(
            ParameterSetName = "NewRelease",
            Mandatory = true,
            Position = 1)]
        [ValidateNotNullOrEmpty]
        public string AssemblyFilePath { get; set; }

        [Parameter(
            ParameterSetName = "NewRelease",
            Mandatory = true)]
        public string UserName { get; set; }

        [Parameter(
            ParameterSetName = "NewRelease",
            Mandatory = true)]
        public string UserEmail { get; set; }

        [Parameter]
        public string UserPassword { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(UserPassword))
            {
                WriteWarning($"The {nameof(UserPassword)} is empty and DefaultCredentials will be used");
            }

            var gitHelper = new GitRepositoryHelper(RepositoryPath);
            var assemblyHelper = new AssemblyVersionFileHelper();
            
            // Assembly
            var content = assemblyHelper.ReadAssemblyFileContent(AssemblyFilePath);
            var readVersion = assemblyHelper.ReadAssemblyVersion(content);
            WriteVerbose($"Found Version {readVersion}");
            var newVersion = assemblyHelper.IncrementAssemblyVersionRevision(readVersion);
            WriteVerbose($"Increment Version to {newVersion}");
            assemblyHelper.WriteAssemblyVersionToPath(AssemblyFilePath, newVersion);

            // Git
            WriteVerbose($"Commit File {AssemblyFilePath}");
            gitHelper.CommitFile(AssemblyFilePath, UserName, UserEmail, $"chore(release): {newVersion}");
            WriteVerbose($"Create Tag {newVersion}");
            gitHelper.CreateTag(newVersion);

            // Push
            WriteVerbose($"Push Tag {newVersion}");
            gitHelper.PushTag(newVersion, UserName, UserPassword);
            var branch = gitHelper.Repository.Head;
            WriteVerbose($"Push Branch {branch.Name} to Remote origin");
            gitHelper.PushBranch(branch, UserName, UserPassword);
        }
    }
}