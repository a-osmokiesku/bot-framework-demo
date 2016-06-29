using TeamCitySharper.ActionTypes;

namespace DemoBot.TeamCityIntegration
{
    public static class BuildsExtention
    {
        public static void Add2QueueByConfigurationAndBranch(this IBuilds source, string configId, string branch)
        {
            source.Add2QueueBuildByBuildConfigId(string.Format("{0}&branchName={1}", configId, branch));
        }
    }
}