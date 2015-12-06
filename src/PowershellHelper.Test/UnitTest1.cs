using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using PowershellHelper.Services;
using Xunit;

namespace PowershellHelper.Test
{
    public class UnitTest1
    {
        private AssemblyVersionHelper AssemblyVersionHelper { get; } = new AssemblyVersionHelper();

        private readonly Random _random = new Random();

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion\src\AssemblyInfo.cs", "5.5.5.6")]
        public void TestSetAssemblyVersion(string assemblyPath, string version)
        {
            AssemblyVersionHelper.WriteAssemblyVersion(assemblyPath, version);
            Assert.Contains(version, File.ReadAllText(assemblyPath));
        }

        [Theory]
        [InlineData("[assembly: AssemblyVersion(\"1.1.1.1\")]\r\n[assembly: AssemblyFileVersion(\"1.1.1.1\")] \r\n","1.1.1.1")]
        public void TestReadAssemblyVersion(string content, string version)
        {
            var assemblyVersion = AssemblyVersionHelper.ReadAssemblyVersion(content);
            Assert.Equal(version, assemblyVersion);
        }

        [Theory]
        [InlineData("1.1.1.1", "1.1.2.1")]
        public void TestIncrementBuild(string version, string result)
        {
            Assert.Equal(AssemblyVersionHelper.IncrementAssemblyVersionBuild(version), result);
        }

        [Theory]
        [InlineData("1.1.1.1", "1.1.1.2")]
        public void TestIncrementRevision(string version, string result)
        {
            Assert.Equal(AssemblyVersionHelper.IncrementAssemblyVersionRevision(version), result);
        }

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion")]
        public void PushHeadRepository(string repository)
        {
            var gitHelper = new GitHelper(repository);
            gitHelper.PushHeadBranch("opiskull","myPassword");
        }

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion")]
        public void PushTag(string repository)
        {
            var gitHelper = new GitHelper(repository);
            var tag = string.Join(".", new []
            {
                _random.Next(1000),
                _random.Next(1000),
                _random.Next(1000),
                _random.Next(1000)
            });
            gitHelper.CreateTag(tag);
            gitHelper.PushTag("opiskull", "myPassword",tag);
        }
    }
}
