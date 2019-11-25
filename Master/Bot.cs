using DALModels;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using BotModule;
using Discord.Commands;

namespace Master
{
    public class Bot
    {
        private const string ConfigDir = "config";
        private const string MasterConfigFile = "master.json";

        private static readonly string MasterConfigPath = Path.Combine(ConfigDir, MasterConfigFile);

        private readonly Configuration masterConfig;

        private DiscordSocketClient client;
        private CommandHandler commandHandler;
        private CommandService commandService;

        public IUserStorage UserStorage { get; }

        public List<BaseModule> Modules { get; } = new List<BaseModule>();

        public Bot(IUserStorage userStorage) {
            UserStorage = userStorage;

            if (!Directory.Exists(ConfigDir))
                Directory.CreateDirectory(ConfigDir);

            if (!File.Exists(MasterConfigPath)) {
                File.WriteAllText(MasterConfigPath, JsonConvert.SerializeObject(new Configuration()));

                //TODO: use some logging system (via ILogger interface)
                Console.WriteLine("No master config, creating empty, please fill it or the bot will be useless :c");
            } else {
                masterConfig = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(MasterConfigPath));
            }

            if (masterConfig != null && string.IsNullOrEmpty(masterConfig.DiscordToken))
                Console.WriteLine("No token found in the master config, please fill it or the bot will be useless :c");
        }

        public async Task Run() {
            //client init
            client = new DiscordSocketClient();

            //TODO: use some loggins system (via ILogger interface)
            client.Log += msg => {
                Console.WriteLine(msg.ToString());
                return Task.CompletedTask;
            };

            await client.LoginAsync(TokenType.Bot, masterConfig.DiscordToken).ConfigureAwait(false);
            await client.StartAsync().ConfigureAwait(false);

            //command handler init
            commandService = new CommandService();
            commandHandler = new CommandHandler(client, commandService);

            //load (bot)modules
            Modules.ForEach(async m => await m.RegisterCommands(commandService).ConfigureAwait(false));
        }
    }
}