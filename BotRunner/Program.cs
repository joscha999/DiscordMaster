using Master;
using System;
using JsonStorage;
using System.IO;
using PingCommandModule;
using QuestionList;

namespace BotRunner
{
    public class Program
    {
        public static void Main(string[] args) {
            Console.WriteLine("Running bot, press enter to exit");

            var bot = new Bot(new JsonUserStorage(Path.Combine("storage", "users.json")));
            bot.Modules.Add(new PingCommand()); //TODO: a way to auto load and configure which modules to load (possibly from assembly)
            bot.Modules.Add(new QuestionListCommand());
            bot.Run().GetAwaiter().GetResult();

            Console.ReadLine();
        }
    }
}