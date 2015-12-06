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
            _pushOptions = CreatePushOptions(username, password);
        }

        private PushOptions CreatePushOptions(string username, string password)
        {
            return new PushOptions
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

        public void PushHeadBranch(string origin = "origin")
        {
            if (Repository.Head.IsTracking)
            {
                Repository.Network.Push(Repository.Head, _pushOptions);
            }
            else
            {
                Push(Repository.Head.CanonicalName);
            }
        }
    }
}