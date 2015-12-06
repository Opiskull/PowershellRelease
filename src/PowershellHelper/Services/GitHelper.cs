﻿using System;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace PowershellHelper.Services
{
    public class GitHelper
    {
        private string RepositoryPath { get; }

        public GitHelper(string repositoryPath)
        {
            RepositoryPath = repositoryPath;
        }

        public void CommitFile(string filePath, string userName, string userEmail, string commitMessage)
        {
            using (var repository = new Repository(RepositoryPath))
            {
                repository.Stage(filePath, new StageOptions());
                var author = new Signature(userName, userEmail, DateTimeOffset.Now);
                repository.Commit(commitMessage, author, author, new CommitOptions());
            }
        }

        public void CreateTag(string tag)
        {
            using (var repository = new Repository(RepositoryPath))
            {
                repository.ApplyTag(tag);
            }
        }

        public string HeadBranchName()
        {
            using (var repository = new Repository(RepositoryPath))
            {
                return repository.Head.Name;
            }
        }

        private void Push(string username, string password, Action<Repository, PushOptions> repositoryPush)
        {
            using (var repository = new Repository(RepositoryPath))
            {
                var pushOptions = new PushOptions()
                {
                    CredentialsProvider = (url, userNameFromUrl, types) => CreateCredentials(username, password)
                };
                repositoryPush(repository, pushOptions);
            }
        }

        private Credentials CreateCredentials(string username, string password)
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

        public void PushTag(string username, string password, string tag, string origin = "origin")
        {
            Push(username, password, (repository, pushOptions) =>
            {
                var remote = repository.Network.Remotes[origin];
                repository.Network.Push(remote, repository.Tags[tag].CanonicalName, pushOptions);
            });
        }

        public void PushHeadBranch(string username, string password, string origin = "origin")
        {
            Push(username, password, (repository, pushOptions) =>
            {
                if (repository.Head.IsTracking)
                {
                    repository.Network.Push(repository.Head, pushOptions);
                }
                else
                {
                    var remote = repository.Network.Remotes[origin];
                    repository.Network.Push(remote, repository.Head.CanonicalName, pushOptions);
                }
            });
        }
    }
}