using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.New, "FileCommit",
        DefaultParameterSetName = "FileCommit")]
    public class NewFileCommit : PSCmdlet
    {
        [Parameter(
            ParameterSetName = "FileCommit",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string FilePath { get; set; }

        [Parameter(
            ParameterSetName = "FileCommit",
            Mandatory = true,
            Position = 1)]
        [ValidateNotNullOrEmpty]
        public string RepositoryPath { get; set; }

        [Parameter(
            ParameterSetName = "FileCommit",
            Mandatory = true)]
        public string UserName { get; set; }

        [Parameter(
            ParameterSetName = "FileCommit",
            Mandatory = true)]
        public string UserEmail { get; set; }

        [Parameter(
            ParameterSetName = "FileCommit",
            Mandatory = true)]
        public string Message { get; set; }

        [Parameter]
        public string Tag { get; set; }

        [Parameter]
        public string CheckOutBranchBeforeCommit { get; set; }

        protected override void ProcessRecord()
        {
            var gitHelper = new GitRepositoryHelper(RepositoryPath, UserName);

            if (!string.IsNullOrWhiteSpace(CheckOutBranchBeforeCommit))
            {
                WriteVerbose($"Checking out {CheckOutBranchBeforeCommit}");
                gitHelper.CheckOut(CheckOutBranchBeforeCommit);
            }

            WriteVerbose($"CommitFile {FilePath} as {UserName}<{UserEmail}> with {Message}");
            gitHelper.CommitFile(FilePath, UserName, UserEmail, Message);
            if (!string.IsNullOrWhiteSpace(Tag))
            {
                WriteVerbose($"CreateTag {Tag}");
                gitHelper.CreateTag(Tag);
            }
        }
    }
}