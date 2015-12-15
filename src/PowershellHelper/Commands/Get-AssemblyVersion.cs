using System.Collections.Generic;
using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Get, "AssemblyVersion",
        DefaultParameterSetName = "AssemblyVersion")]
    public class GetAssemblyVersion : PSCmdlet
    {
        private AssemblyVersionFileHelper AssemblyFileHelper { get; } = new AssemblyVersionFileHelper();

        [Parameter(
            ParameterSetName = "AssemblyVersion",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string AssemblyFilePath { get; set; }

        [Parameter]
        public SwitchParameter AssemblyVersion { get; set; }

        [Parameter]
        public SwitchParameter AssemblyFileVersion { get; set; }

        [Parameter]
        public SwitchParameter AssemblyInformationalVersion { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose($"Get-AssemblyVersion in {AssemblyFilePath}");
            var content = AssemblyFileHelper.ReadAssemblyFileContent(AssemblyFilePath);

            if (AssemblyVersion || AssemblyFileVersion || AssemblyInformationalVersion)
            {
                var output = new Dictionary<string, string>();
                if (AssemblyFileVersion)
                {
                    output.Add($"{nameof(AssemblyFileVersion)}", AssemblyFileHelper.ReadAssemblyFileVersion(content));
                }
                if (AssemblyInformationalVersion)
                {
                    output.Add($"{nameof(AssemblyInformationalVersion)}",
                        AssemblyFileHelper.ReadAssemblyInformationalVersion(content));
                }
                if (AssemblyVersion)
                {
                    output.Add($"{nameof(AssemblyVersion)}", AssemblyFileHelper.ReadAssemblyVersion(content));
                }
                WriteObject(output);
            }
            else
            {
                WriteWarning($"{nameof(AssemblyFileVersion)} OR {nameof(AssemblyInformationalVersion)} OR {nameof(AssemblyVersion)} should be set!");
            }
        }
    }
}