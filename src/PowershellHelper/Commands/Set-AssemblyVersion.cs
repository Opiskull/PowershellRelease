using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Set, "AssemblyVersion",
        DefaultParameterSetName = "AssemblyFilePath")]
    public class SetAssemblyVersion : PSCmdlet
    {
        private AssemblyVersionFileHelper AssemblyVersionFileHelper { get; } = new AssemblyVersionFileHelper();

        [Parameter(
            ParameterSetName = "AssemblyFilePath",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string AssemblyFilePath { get; set; }

        [Parameter(Position = 1)]
        public string NewVersion { get; set; }

        [Parameter]
        public SwitchParameter IncrementBuild { get; set; }

        [Parameter]
        public SwitchParameter IncrementRevision { get; set; }

        protected override void ProcessRecord()
        {
            var newVersion = NewVersion ?? AssemblyVersionFileHelper.ReadAssemblyVersionFromPath(AssemblyFilePath);
            if (IncrementBuild)
            {
                WriteVerbose($"{nameof(IncrementBuild)} {newVersion}");
                newVersion = AssemblyVersionFileHelper.IncrementAssemblyVersionBuild(newVersion);
            }
            if (IncrementRevision)
            {
                WriteVerbose($"{nameof(IncrementRevision)} {newVersion}");
                newVersion = AssemblyVersionFileHelper.IncrementAssemblyVersionRevision(newVersion);
            }
            WriteVerbose($"Set AssemblyVersion in {AssemblyFilePath} to {newVersion}");
            AssemblyVersionFileHelper.WriteAssemblyVersionToPath(AssemblyFilePath, newVersion);
            WriteObject(newVersion);
        }
    }
}