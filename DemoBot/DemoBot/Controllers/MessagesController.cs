using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DemoBot.TeamCityIntegration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

namespace DemoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private BuildParam state = new BuildParam();

        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                var data = message.GetBotUserData<BuildParam>("build");
                if (data != null) state = data;
                var reply = await Reply(message.Text);
                var msg = message.CreateReplyMessage(string.Format(reply));
                msg.SetBotUserData("build", state);
                return msg;
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private async Task<string> Reply(string msg)
        {
            var statements = msg.Split(' ');
            if (IsPresent(statements, "help"))
            {
                return "TODO: HELP INFO.";
            }

            if (statements.Length <= 3)
            {
                return "Sorry, i am not understand you.";
            }

            if (IsPresent(statements, "project")) state.ProjectName = NextTo(statements, "project");
            if (IsPresent(statements, "configuration")) state.ConfigName = NextTo(statements, "configuration");
            if (IsPresent(statements, "branch")) state.BranchName = NextTo(statements, "branch");

            return await state.BuildResult();
        }

        private bool IsPresent(string[] str, string target)
        {
            return str.Any(item => item.ToLower().Equals(target,StringComparison.InvariantCultureIgnoreCase));
        }

        private string NextTo(string[] str, string pat)
        {
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i].ToLower() == pat) return str[i + 1];
            }
            return "";
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}