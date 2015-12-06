using System.Management.Automation;
using System.Security;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Push, "Repository",
      DefaultParameterSetName = "VersionCommit")]
    public class PushRepository : PSCmdlet
    {
        [Parameter(
            ParameterSetName = "VersionCommit",
            Mandatory = true,
            ValueFromPipeline = true, Position = 0)]
        public string RepositoryPath { get; set; }

        [Parameter(
            Mandatory = true)]
        public string UserName { get; set; }

        [Parameter]
        public string UserPassword { get; set; }

        [Parameter]
        public string[] Tags { get; set; }

        protected override void ProcessRecord()
        {
            var gitHelper = new GitHelper(RepositoryPath);
            if (Tags?.Length > 0)
            {
                foreach (var tag in Tags)
                {
                    WriteVerbose($"Push Tag {tag}");
                    gitHelper.PushTag(UserName,UserPassword,tag);
                }
            }
            var branch = gitHelper.HeadBranchName();
            WriteVerbose($"Push Branch {branch} to Remote origin");
            gitHelper.PushHeadBranch(UserName,UserPassword);
        }
    }
}
