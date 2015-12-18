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
            var gitHelper = new GitRepositoryHelper(repositoryPath);
            gitHelper.Push(gitHelper.Repository.Head.Name, "opiskull", "myPassword");
        }

        [Theory]
        [InlineData(@"C:\Github\autoincrement-assemblyversion")]
        public void PushTag(string repositoryPath)
        {
            var gitHelper = new GitRepositoryHelper(repositoryPath);
            var tag = string.Join(".", new[]
            {
                Random.Next(1000),
                Random.Next(1000),
                Random.Next(1000),
                Random.Next(1000)
            });
            gitHelper.CreateTag(tag);
            gitHelper.PushTag(tag, "opiskull", "myPassword");
        }
    }
}