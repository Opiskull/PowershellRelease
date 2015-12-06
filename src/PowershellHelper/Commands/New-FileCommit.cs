using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.New, "FileCommit",
        DefaultParameterSetName = "VersionCommit")]
    public class NewAssemblyVersionCommit : PSCmdlet
    {
        [Parameter(
            ParameterSetName = "VersionCommit",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string FilePath { get; set; }

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
            ParameterSetName = "VersionCommit",
            Mandatory = true)]
        public string Message { get; set; }

        [Parameter]
        public string Tag { get; set; }

        protected override void ProcessRecord()
        {
            var gitHelper = new GitRepositoryHelper(RepositoryPath, UserName);
            WriteVerbose("CommitFile");
            gitHelper.CommitFile(FilePath, UserName, UserEmail, Message);
            if (!string.IsNullOrWhiteSpace(Tag))
            {
                WriteVerbose($"CreateTag {Tag}");
                gitHelper.CreateTag(Tag);
            }
        }
    }
}