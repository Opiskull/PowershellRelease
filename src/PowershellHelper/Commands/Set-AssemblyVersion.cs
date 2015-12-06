using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Set, "AssemblyVersion",
        DefaultParameterSetName = "FileName")]
    public class SetAssemblyVersion : PSCmdlet
    {
        private AssemblyVersionFileHelper AssemblyVersionFileHelper { get; } = new AssemblyVersionFileHelper();

        [Parameter(
            ParameterSetName = "FileName",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string FilePath { get; set; }

        [Parameter(Position = 1)]
        public string NewVersion { get; set; }

        [Parameter]
        public SwitchParameter IncrementBuild { get; set; }

        [Parameter]
        public SwitchParameter IncrementRevision { get; set; }

        protected override void ProcessRecord()
        {
            var newVersion = NewVersion ?? AssemblyVersionFileHelper.ReadAssemblyVersionFromFile(FilePath);
            if (IncrementBuild)
            {
                WriteVerbose($"IncrementBuild {newVersion}");
                newVersion = AssemblyVersionFileHelper.IncrementAssemblyVersionBuild(newVersion);
            }
            if (IncrementRevision)
            {
                WriteVerbose($"IncrementRevision {newVersion}");
                newVersion = AssemblyVersionFileHelper.IncrementAssemblyVersionRevision(newVersion);
            }
            WriteVerbose($"Set AssemblyVersion in {FilePath} to {newVersion}");
            AssemblyVersionFileHelper.WriteAssemblyVersionToFile(FilePath, newVersion);
            WriteObject(newVersion);
        }
    }
}