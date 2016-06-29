using System;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyHttp.Infrastructure;
using TeamCitySharper;
using TeamCitySharper.Connection;
using TeamCitySharper.Locators;

namespace DemoBot.TeamCityIntegration
{
    public class TCClient
    {
        private const string Login = "Artur_Osmokiesku";
        private const string Password = "Jcvjrtcre93";
        private const string Host = "137.116.205.47";

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
    }
}