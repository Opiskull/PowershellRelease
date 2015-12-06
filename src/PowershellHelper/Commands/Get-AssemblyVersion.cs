using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Get, "AssemblyVersion",
      DefaultParameterSetName = "AssemblyVersion")]
    public class GetAssemblyVersion : PSCmdlet
    {
        private AssemblyVersionHelper AssemblyVersionHelper { get; } = new AssemblyVersionHelper();

        [Parameter(
          ParameterSetName = "AssemblyVersion",
          Mandatory = true,
          ValueFromPipeline = true, Position = 0)]
        public string FilePath { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose($"Get-AssemblyVersion in {FilePath}");
            WriteObject(AssemblyVersionHelper.ReadAssemblyVersionFromFile(FilePath));
        }
    }
}
