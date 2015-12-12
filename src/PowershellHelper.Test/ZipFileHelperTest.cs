using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using PowershellHelper.Services;
using Xunit;

namespace PowershellHelper.Test
{
    public class ZipFileHelperTest
    {
        private ZipFileHelper Helper { get; } = new ZipFileHelper();

        [Theory]
        [InlineData(@"C:\Github\PowershellRelease\src\PowershellHelper\bin\Debug\testArchive.zip", @"C:\Github\PowershellRelease\src\PowershellHelper.Test")]
        public void CreateZipFileFromFolder(string archiveDestination, string inputFolder)
        {
            Helper.CreateZipFileFromDirectory(archiveDestination,inputFolder);
            Assert.True(File.Exists(archiveDestination));
        }

        [Theory]
        [InlineData(@"C:\Github\PowershellRelease\src\PowershellHelper\bin\Debug\testArchive.zip")]
        public void PushTag(string archiveDestination)
        {
            Helper.CreateZipFile(archiveDestination, InputFilesData);
            Assert.True(File.Exists(archiveDestination));
        }

        public IEnumerable<string> InputFilesData => Directory.EnumerateFiles(@"C:\Github\PowershellRelease\src\PowershellHelper\Commands");
    }
}