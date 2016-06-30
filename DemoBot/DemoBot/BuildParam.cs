using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DemoBot.TeamCityIntegration;

namespace DemoBot
{
    public class BuildParam
    {
        public BuildParam()
        {
            BranchName = "master";
        }

        public string ProjectName { get; set; }
        public string ConfigName { get; set; }
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