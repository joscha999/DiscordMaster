using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService service;

        public CommandHandler(DiscordSocketClient client, CommandService service) {
            this.client = client;
            this.service = service;

            client.MessageReceived += HandleMessage;
        }

        private async Task HandleMessage(SocketMessage message) {
            if (!(message is SocketUserMessage msg))
                return;

            if (msg.Author.IsBot)
                return;

            int argPos = 0;

            //either start with ! or with @botname
            if (!(msg.HasCharPrefix('!', ref argPos) || msg.HasMentionPrefix(client.CurrentUser, ref argPos)))
                return;

            var context = new SocketCommandContext(client, msg);

            await service.ExecuteAsync(context, argPos, null).ConfigureAwait(false);
        }
    }
}