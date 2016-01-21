using System;
using LibGit2Sharp;
using System.Linq;

namespace PowershellHelper.Services
{
    public class GitRepositoryHelper
    {
        private const string REFS_HEADS = "refs/heads/";

        public Repository Repository { get; }

        public string UserName { get; }

        public string Password { get; }

        public GitRepositoryHelper(string repositoryPath, string username = "", string password = "")
        {
            UserName = username;
            Password = password;
            Repository = new Repository(repositoryPath);
        }

        private PushOptions CreatePushOptions()
        {
            return new PushOptions
            {
                CredentialsProvider = (url, userNameFromUrl, types) =>
                {
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        return new DefaultCredentials();
                    }
                    return new UsernamePasswordCredentials
                    {
                        Username = UserName,
                        Password = Password
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

        public void PushBranch(string branchName)
        {
            var branch = Repository.Branches[branchName];
            Push($"{branch.CanonicalName}:{branch.CanonicalName}");
        }

        private void Push(string refSpec)
        {
            var remote = Repository.Network.Remotes.Single();
            Repository.Network.Push(remote, refSpec, CreatePushOptions());
        }

        public void PushTag(string tagName)
        {
            var tag = Repository.Tags[tagName];
            Push($"{tag.CanonicalName}:{tag.CanonicalName}");
        }

        public void CheckOut(string canonicalName, string origin = "origin")
        {
            Branch branch = null;
            if (canonicalName.StartsWith(REFS_HEADS))
            {
                var branchName = canonicalName.Replace(REFS_HEADS, "");
                var remoteBranchName = $"{origin}/{branchName}";
                var trackedBranch = Repository.Branches[remoteBranchName];
                var localBranch = Repository.CreateBranch(branchName, remoteBranchName);
                branch = Repository.Branches.Update(localBranch, br => {
                    br.TrackedBranch = trackedBranch.CanonicalName;
                    });
                Repository.Checkout(branch);
            }
            else
            {
                Repository.Checkout(canonicalName);
            }
        }
    }
}