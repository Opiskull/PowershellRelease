using System;
using LibGit2Sharp;

namespace PowershellHelper.Services
{
    public class GitRepositoryHelper
    {
        public Repository Repository { get; }

        private readonly PushOptions _pushOptions;

        public GitRepositoryHelper(string repositoryPath, string username, string password = "")
        {
            Repository = new Repository(repositoryPath);
            _pushOptions = new PushOptions
            {
                CredentialsProvider = (url, userNameFromUrl, types) =>
                {
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return new DefaultCredentials();
                    }
                    return new UsernamePasswordCredentials
                    {
                        Username = username,
                        Password = password
                    };
                }
            };
        }

        public string LatestCommitSha()
        {
            return Repository.Head.Tip.Sha;
        }

        public void CommitFile(string filePath, string userName, string userEmail, string commitMessage)
        {
            Repository.Stage(filePath, new StageOptions());
            var author = new Signature(userName, userEmail, DateTimeOffset.Now);
            Repository.Commit(commitMessage, author, author, new CommitOptions());
        }

        public void CreateTag(string tag)
        {
            Repository.ApplyTag(tag);
        }

        public void Push(string canonicalName, string origin = "origin")
        {
            var remote = Repository.Network.Remotes[origin];
            Repository.Network.Push(remote, canonicalName, _pushOptions);
        }

        public void PushTag(string tag, string origin = "origin")
        {
            Push(Repository.Tags[tag].CanonicalName);
        }

        public void PushBranch(Branch branch, string origin = "origin")
        {
            if (branch.IsTracking)
            {
                Repository.Network.Push(branch, _pushOptions);
            }
            else
            {
                Push(branch.CanonicalName);
            }
        }
    }
}