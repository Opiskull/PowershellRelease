using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.New, "ZipFile")]
    public class NewZipFile : PSCmdlet
    {
        private ZipFileHelper ZipHelper { get; } = new ZipFileHelper();

        [Parameter(
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string ArchiveDestinationPath { get; set; }

        [Parameter]
        public string[] InputFiles { get; set; }

        [Parameter]
        public string InputFolder { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(InputFolder) && InputFiles?.Length == 0)
            {
                WriteError(new ErrorRecord(
                    new ArgumentException(
                        $"At least 1 {nameof(InputFiles)} or an {nameof(InputFolder)} should be specified!"),
                    "NoInputFound",
                    ErrorCategory.InvalidData, this));
                return;
            }

            var archiveDestinationPath = SessionState.Path.GetUnresolvedProviderPathFromPSPath(ArchiveDestinationPath);
            WriteVerbose($"Create ZipArchive at {archiveDestinationPath}");
            if (InputFiles?.Length > 0)
            {
                WriteVerbose($"Using {nameof(InputFiles)} to create ZipArchive");
                var files = new List<string>();
                foreach (var file in InputFiles)
                {
                    ProviderInfo provider;
                    files.AddRange(SessionState.Path.GetResolvedProviderPathFromPSPath(file, out provider));
                }
                WriteVerbose($"Following Files will be copied into {archiveDestinationPath}");
                foreach (var file in files)
                {
                    WriteVerbose(file);
                }
                ZipHelper.CreateZipFile(archiveDestinationPath, files, Force);
            }
            else
            {
                WriteVerbose($"Using {nameof(InputFolder)} to create ZipArchive");
                var inputFolder = SessionState.Path.GetUnresolvedProviderPathFromPSPath(InputFolder);
                WriteVerbose($"Folder {inputFolder} will be copied into {archiveDestinationPath}");
                ZipHelper.CreateZipFileFromDirectory(archiveDestinationPath, inputFolder);
            }
        }
    }
}
