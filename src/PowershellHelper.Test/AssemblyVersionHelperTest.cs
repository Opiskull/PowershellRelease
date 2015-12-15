using System.IO;
using PowershellHelper.Services;
using Xunit;

namespace PowershellHelper.Test
{
    public class AssemblyVersionHelperTest
    {
        private AssemblyVersionFileHelper AssemblyVersionFileHelper { get; } = new AssemblyVersionFileHelper();

        [Theory]
        [InlineData("[assembly: AssemblyVersion(\"1.1.1.1\")]\r\n[assembly: AssemblyFileVersion(\"1.1.1.1\")]\r\n", "5.5.5.6")]
        public void TestSetAssemblyVersion(string assemblyContent, string version)
        {
            assemblyContent = AssemblyVersionFileHelper.WriteAssemblyVersion(assemblyContent, version);
            Assert.Contains(version, assemblyContent);
        }

        [Theory]
        [InlineData("[assembly: AssemblyVersion(\"1.1.1.1\")]\r\n[assembly: AssemblyFileVersion(\"1.1.1.1\")]\r\n","1.1.1.1")]
        [InlineData("//[assembly: AssemblyVersion(\"1.1.1.1\")]\r\n//[assembly: AssemblyFileVersion(\"1.1.1.1\")]\r\n", "")]
        [InlineData("//[assembly: AssemblyVersion(\"1.1.1.1\")]\r\n[assembly: AssemblyVersion(\"1.2.3.4\")]\r\n", "1.2.3.4")]
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
