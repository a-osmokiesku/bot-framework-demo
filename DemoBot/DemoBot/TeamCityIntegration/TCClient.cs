using TeamCitySharper;

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
            client.Connect(Login,Password);
        }

        public void RunBuild(string configName)
        {
            var foo = client.BuildConfigs.ByConfigurationName(configName).Id;
            client.Builds.Add2QueueBuildByBuildConfigId(foo);
        }

        public bool IsBuildConfigurationExist(string configName)
        {
            var buildConfig = client.BuildConfigs.ByConfigurationName(configName);
            return buildConfig != null;
        }
    }
}