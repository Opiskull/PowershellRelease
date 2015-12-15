using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Set, "AssemblyVersion",
        DefaultParameterSetName = "SetAssemblyVersion")]
    public class SetAssemblyVersion : PSCmdlet
    {
        private AssemblyVersionFileHelper _assemblyVersionFileHelper { get; } = new AssemblyVersionFileHelper();

        [Parameter(
            ParameterSetName = "SetAssemblyVersion",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string AssemblyFilePath { get; set; }

        [Parameter(ParameterSetName = "SetAssemblyVersion",
            Position = 1)]
        public string NewVersion { get; set; }

        [Parameter]
        public SwitchParameter IncrementBuild { get; set; }

        [Parameter]
        public SwitchParameter IncrementRevision { get; set; }

        protected override void ProcessRecord()
        {
            var content = _assemblyVersionFileHelper.ReadAssemblyFileContent(AssemblyFilePath);
            var newVersion = NewVersion ?? _assemblyVersionFileHelper.ReadAssemblyVersion(content);
            if (IncrementBuild)
            {
                WriteVerbose($"{nameof(IncrementBuild)} {newVersion}");
                newVersion = _assemblyVersionFileHelper.IncrementAssemblyVersionBuild(newVersion);
            }
            if (IncrementRevision)
            {
                WriteVerbose($"{nameof(IncrementRevision)} {newVersion}");
                newVersion = _assemblyVersionFileHelper.IncrementAssemblyVersionRevision(newVersion);
            }
            WriteVerbose($"Set AssemblyVersion in {AssemblyFilePath} to {newVersion}");
            _assemblyVersionFileHelper.WriteAssemblyVersionToPath(AssemblyFilePath, newVersion);
            WriteObject(newVersion);
        }
    }
}