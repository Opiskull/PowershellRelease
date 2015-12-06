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
        public string FilePath { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose($"Get-AssemblyVersion in {FilePath}");
            WriteObject(AssemblyVersionFileHelper.ReadAssemblyVersionFromFile(FilePath));
        }
    }
}