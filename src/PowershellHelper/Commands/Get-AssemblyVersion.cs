using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Get, "AssemblyVersion",
        DefaultParameterSetName = "AssemblyVersion")]
    public class GetAssemblyVersion : PSCmdlet
    {
        private AssemblyVersionFileHelper AssemblyVersionFileHelper { get; } = new AssemblyVersionFileHelper();

        [Parameter(
            ParameterSetName = "AssemblyVersion",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string AssemblyFilePath { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose($"Get-AssemblyVersion in {AssemblyFilePath}");
            var version = AssemblyVersionFileHelper.ReadAssemblyVersionFromPath(AssemblyFilePath);
            WriteVerbose($"Found version {version}");
            WriteObject(version);
        }
    }
}