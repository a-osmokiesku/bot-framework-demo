using System;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DemoBot.TeamCityIntegration;
using Microsoft.Bot.Builder.Dialogs;

namespace DemoBot
{
    [Serializable]
    public class BuildDialog : IDialog<BuildParam>
    {
        private BuildParam state = new BuildParam();

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageRecivedAsync);
        }

        public async Task MessageRecivedAsync(IDialogContext context, IAwaitable<Message> argument)
        {
            var message = await argument;
            PromptDialog.Confirm(context, Subscribe,
                string.Format("Are you sure to build branch {0} on configuration {1}?", state.BranchName, state.ConfigName));

            var reply = await Reply(message.Text);
            await context.PostAsync(reply);
        }

        private async Task Subscribe(IDialogContext context, IAwaitable<bool> result)
        {
            var answer = await result;
            if (answer)
            {
                context.Wait(MessageRecivedAsync);
            }
            else
            {
                await context.PostAsync("Build canceled");
            }
            context.Wait(MessageRecivedAsync);
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
            return str.Any(item => item.ToLower().Equals(target, StringComparison.InvariantCultureIgnoreCase));
        }

        private string NextTo(string[] str, string pat)
        {
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i].ToLower() == pat) return str[i + 1];
            }
            return "";
        }
    }
}