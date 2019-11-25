using BotModule;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace PingCommandModule
{
    public class PingCommand : BaseModule
    {
        public override async Task RegisterCommands(CommandService service) {
            await service.AddModuleAsync(typeof(PingModule), null).ConfigureAwait(false);
        }
    }

    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Pongs back.")]
        public async Task PingAsync() => await Context.Channel.SendMessageAsync("Pong!").ConfigureAwait(false);
    }
}