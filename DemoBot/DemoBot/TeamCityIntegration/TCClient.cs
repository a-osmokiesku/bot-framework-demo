using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyHttp.Infrastructure;
using TeamCitySharper;
using TeamCitySharper.Connection;
using TeamCitySharper.DomainEntities;
using TeamCitySharper.Locators;

namespace DemoBot.TeamCityIntegration
{
    public class TCClient
    {
        private const string Login = "Artur_Osmokiesku";
        private const string Password = "1234";
        private const string Host = "172.31.8.174:1000";

        private readonly TeamCityClient client;

        public TCClient()
        {
            client = new TeamCityClient(Host);
            client.Connect(Login, Password);
        }

        public void RunBuild(string configName)
        {
            var configId = client.BuildConfigs.ByConfigurationName(configName).Id;
            client.Builds.Add2QueueBuildByBuildConfigId(configId);
        }

        public void RunBuild(string configName, string branchName)
        {
            var configId = client.BuildConfigs.ByConfigurationName(configName).Id;
            client.Builds.Add2QueueByConfigurationAndBranch(configId, branchName);
        }

        public bool IsBuildConfigurationExist(string configName)
        {
            try
            {
                client.BuildConfigs.ByConfigurationName(configName);
            }
            catch (HttpException)
            {
                return false;
            }
            return true;
        }

        public bool IsProjectExist(string projectName)
        {
            try
            {
                client.Projects.ByName(projectName);
            }
            catch (HttpException)
            {
                return false;
            }
            return true;
        }
    }
}