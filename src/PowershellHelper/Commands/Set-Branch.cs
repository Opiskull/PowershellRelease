using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Set, "Branch",
        DefaultParameterSetName = "SetBranch")]
    public class SetBranch : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string RepositoryPath { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Branch { get; set; }

        protected override void ProcessRecord()
        {
            var githelper = new GitRepositoryHelper(RepositoryPath);
            WriteVerbose($"Check out {Branch}");
            githelper.CheckOut(Branch);
        }
    }
}
