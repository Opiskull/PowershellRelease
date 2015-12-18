﻿using System.Management.Automation;
using System.Security;
using PowershellHelper.Services;

namespace PowershellHelper.Commands
{
    [Cmdlet(VerbsCommon.Push, "Repository",
        DefaultParameterSetName = "PushRepository")]
    public class PushRepository : PSCmdlet
    {
        [Parameter(
            ParameterSetName = "PushRepository",
            Mandatory = true,
            Position = 0)]
        [ValidateNotNullOrEmpty]
        public string RepositoryPath { get; set; }

        [Parameter]
        public string UserName { get; set; }

        [Parameter]
        public string UserPassword { get; set; }

        [Parameter]
        public string[] Tags { get; set; }

        protected override void ProcessRecord()
        {
            var gitHelper = new GitRepositoryHelper(RepositoryPath, UserName, UserPassword);
            if (Tags?.Length > 0)
            {
                foreach (var tag in Tags)
                {
                    WriteVerbose($"Push Tag {tag}");
                    gitHelper.PushTag(tag);
                }
            }
            var branch = gitHelper.Repository.Head;
            WriteVerbose($"Push Branch {branch.Name} to Remote origin");
            gitHelper.Push(branch.Name);
        }
    }
}