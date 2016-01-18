using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Get, "LatestCommit",
        DefaultParameterSetName = "LatestCommit")]
    public class GetLatestCommit : PSCmdlet
    {
        [Parameter(
    ParameterSetName = "LatestCommit",
    Mandatory = true,
    Position = 0)]
        [ValidateNotNullOrEmpty]
        public string RepositoryPath { get; set; }

        protected override void ProcessRecord()
        {
            var gitHelper = new GitRepositoryHelper(RepositoryPath);
            WriteObject(gitHelper.LatestCommitSha());
        }
    }
}