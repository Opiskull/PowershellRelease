using System;
using LibGit2Sharp;

namespace PowershellHelper.Services
{
    public class GitRepositoryHelper
    {
        private const string REFS_HEADS = "refs/heads/";

        public Repository Repository { get; }

        public GitRepositoryHelper(string repositoryPath)
        {
            Repository = new Repository(repositoryPath);
        }

        private PushOptions CreatePushOptions(string username = "", string password = "")
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

        public void PushBranch(string localBranch, string username = "", string password = "")
        {
            var branch = Repository.Branches[localBranch];
            PushBranch(branch, username, password);
        }

        public void PushBranch(Branch branch, string username = "", string password = "")
        {
            Repository.Network.Push(branch, CreatePushOptions(username, password));
        }      

        public void PushCurrentBranch(string username = "", string password = "", string origin = "origin")
        {
            PushBranch(Repository.Head,username,password);
        }

        public void PushTag(string tagName, string username = "", string password = "", string origin = "origin")
        {
            var remote = Repository.Network.Remotes[origin];
            var tag = Repository.Tags[tagName];
            Repository.Network.Push(remote, tag.CanonicalName, tag.CanonicalName, CreatePushOptions(username, password));
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
                branch = Repository.Branches.Update(localBranch, br => br.TrackedBranch = trackedBranch.CanonicalName);
                Repository.Checkout(branch);
            }
            else
            {
                Repository.Checkout(canonicalName);
            }
        }
    }
}