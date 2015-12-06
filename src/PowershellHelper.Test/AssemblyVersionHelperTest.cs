using System.IO;
using PowershellHelper.Services;
using Xunit;

namespace PowershellHelper.Test
{
    public class AssemblyVersionHelperTest
    {
        private AssemblyVersionFileHelper AssemblyVersionFileHelper { get; } = new AssemblyVersionFileHelper();

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion\src\AssemblyInfo.cs", "5.5.5.6")]
        public void TestSetAssemblyVersion(string assemblyPath, string version)
        {
            AssemblyVersionFileHelper.WriteAssemblyVersion(assemblyPath, version);
            Assert.Contains(version, File.ReadAllText(assemblyPath));
        }

        [Theory]
        [InlineData("[assembly: AssemblyVersion(\"1.1.1.1\")]\r\n[assembly: AssemblyFileVersion(\"1.1.1.1\")] \r\n","1.1.1.1")]
        public void TestReadAssemblyVersion(string assemblyContent, string version)
        {
            var assemblyVersion = AssemblyVersionFileHelper.ReadAssemblyVersion(assemblyContent);
            Assert.Equal(version, assemblyVersion);
        }

        [Theory]
        [InlineData("1.1.1.1", "1.1.2.1")]
        public void TestIncrementBuild(string version, string result)
        {
            Assert.Equal(AssemblyVersionFileHelper.IncrementAssemblyVersionBuild(version), result);
        }

        [Theory]
        [InlineData("1.1.1.1", "1.1.1.2")]
        public void TestIncrementRevision(string version, string result)
        {
            Assert.Equal(AssemblyVersionFileHelper.IncrementAssemblyVersionRevision(version), result);
        }
    }
}
