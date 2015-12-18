using System;
using LibGit2Sharp;

namespace PowershellHelper.Services
{
    public class GitRepositoryHelper
    {
        public Repository Repository { get; }

        public GitRepositoryHelper(string repositoryPath)
        {
            Repository = new Repository(repositoryPath);
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

        public void PushBranch(string branch, string username = "", string password = "", string origin = "origin")
        {
            Push(Repository.Branches[branch].CanonicalName, username, password, origin);
        }

        public void Push(string canonicalName, string username = "", string password = "", string origin = "origin")
        {
            var pushOptions = new PushOptions
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
            var remote = Repository.Network.Remotes[origin];
            Repository.Network.Push(remote, canonicalName, pushOptions);
        }

        public void PushTag(string tag, string username = "", string password = "", string origin = "origin")
        {
            Push(Repository.Tags[tag].CanonicalName,username,password,origin);
        }

        public void CheckOut(string branch)
        {
            Repository.Checkout(branch);
        }
    }
}