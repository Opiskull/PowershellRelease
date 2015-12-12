using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace PowershellHelper.Services
{
    public class ZipFileHelper
    {
        public void CreateZipFile(string archiveFilePath, IEnumerable<string> inputFileNames, bool force = true)
        {
            using (FileStream zipToOpen = new FileStream(archiveFilePath, force ? FileMode.Create : FileMode.CreateNew))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (var inputFileName in inputFileNames)
                    {
                        archive.CreateEntryFromFile(inputFileName, Path.GetFileName(inputFileName));
                    }
                }
            }
        }

        public void CreateZipFileFromDirectory(string archiveFilePath, string inputFolder)
        {
            ZipFile.CreateFromDirectory(inputFolder,archiveFilePath);
        }

        public void ExtractZipFile(string archivePath, string destinationPath)
        {
            ZipFile.ExtractToDirectory(archivePath,destinationPath);
        }
    }
}
