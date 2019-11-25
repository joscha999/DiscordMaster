using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace BotModule
{
    public abstract class BaseModule
    {
        public abstract Task RegisterCommands(CommandService service);
    }
}