using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Set, "AssemblyVersion",
        DefaultParameterSetName = "FileName")]
    public class SetAssemblyVersion : PSCmdlet
    {
        private AssemblyVersionHelper AssemblyVersionHelper { get; } = new AssemblyVersionHelper();

        [Parameter(
            ParameterSetName = "FileName",
            Mandatory = true,
            Position = 0)]
        public string FilePath { get; set; }

        [Parameter(Position = 1)]
        public string NewVersion { get; set; }

        [Parameter]
        public SwitchParameter IncrementBuild { get; set; }

        [Parameter]
        public SwitchParameter IncrementRevision { get; set; }

        protected override void ProcessRecord()
        {
            var newVersion = NewVersion ?? AssemblyVersionHelper.ReadAssemblyVersionFromFile(FilePath);
            if (IncrementBuild)
            {
                WriteVerbose($"IncrementBuild {newVersion}");
                newVersion = AssemblyVersionHelper.IncrementAssemblyVersionBuild(newVersion);
            }
            if (IncrementRevision)
            {
                WriteVerbose($"IncrementRevision {newVersion}");
                newVersion = AssemblyVersionHelper.IncrementAssemblyVersionRevision(newVersion);
            }
            WriteVerbose($"Set AssemblyVersion in {FilePath} to {newVersion}");
            AssemblyVersionHelper.WriteAssemblyFile(FilePath, newVersion);
            WriteObject(newVersion);
        }
    }
}