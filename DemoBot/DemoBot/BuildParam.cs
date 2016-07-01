using System;
using System.Threading.Tasks;
using DemoBot.TeamCityIntegration;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace DemoBot
{
    [Serializable]
    public class BuildParam
    {
        public BuildParam()
        {
            BranchName = "master";
        }

        public static IForm<BuildParam> BuildForm()
        {
            return new FormBuilder<BuildParam>()
                .Message("Welcome to TeamCity assistent!")
                .OnCompletion(Subscribe)
                .Build();
        }

        private static async Task Subscribe(IDialogContext context, BuildParam data)
        {
            await context.PostAsync(await data.BuildResult());
        }

        [Prompt("What is the target project name?")]
        public string ProjectName { get; set; }

        [Prompt("What is build configuration name?")]
        public string ConfigName { get; set; }

        [Prompt("What is the target branch name?")]
        [Optional]
        public string BranchName { get; set; }

        public async Task<string> BuildResult()
        {
            var client = new TCClient();

            if (!client.IsProjectExist(ProjectName))
            {
                return string.Format("Project with name {0} not found.", ProjectName);
            }

            if (!client.IsBuildConfigurationExist(ConfigName))
            {
                return "I don't understand you. Maybe this configuration not found.";
            }

            client.RunBuild(ConfigName, BranchName);
            return string.Format("Running branch {0} in project {1} on {2}.", BranchName, ProjectName, ConfigName);
        }
    }
}