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
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                var reply = await Reply(message.Text);
                return message.CreateReplyMessage(string.Format(reply));
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private async Task<string> Reply(string msg)
        {
            var client = new TCClient();
            if (!client.IsBuildConfigurationExist(msg))
            {
                return "I don't understand you. Maybe this configuration not found.";
            }

            client.RunBuild(msg);
            return "Build is running.";
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