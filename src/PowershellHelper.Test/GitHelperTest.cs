using System;
using PowershellHelper.Services;
using Xunit;

namespace PowershellHelper.Test
{
    public class GitHelperTest
    {
        private Random Random { get; } = new Random();

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion")]
        public void PushHeadRepository(string repositoryPath)
        {
            var gitHelper = new GitRepositoryHelper(repositoryPath, "opiskull", "myPassword");
            gitHelper.PushBranch(gitHelper.Repository.Head.Name);
        }

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion")]
        public void PushTag(string repositoryPath)
        {
            var gitHelper = new GitRepositoryHelper(repositoryPath, "opiskull", "myPassword");
            var tag = string.Join(".", new[]
            {
                Random.Next(1000),
                Random.Next(1000),
                Random.Next(1000),
                Random.Next(1000)
            });
            gitHelper.CreateTag(tag);
            gitHelper.PushTag(tag);
        }

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion", "refs/heads/test/test")]
        public void CheckoutRef(string repositoryPath, string canonicalName)
        {
            var gitHelper = new GitRepositoryHelper(repositoryPath);
            gitHelper.CheckOut(canonicalName);
            var current = gitHelper.Repository.Head.CanonicalName;
            Assert.Equal(canonicalName, current);
        }
    }
}