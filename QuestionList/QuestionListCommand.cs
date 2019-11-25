using BotModule;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QuestionList
{
    public class QuestionListCommand : BaseModule
    {
        public override async Task RegisterCommands(CommandService service) {
            await service.AddModuleAsync(typeof(QuestionListModule), null).ConfigureAwait(false);
        }
    }

    public static class ListHandler
    {
        private static readonly string StoragePath = Path.Combine("storage", "questions.json");

        public static List<QuestionInfo> Questions { get; }

        static ListHandler() {
            if (!Directory.Exists("storage"))
                Directory.CreateDirectory("storage");

            if (File.Exists(StoragePath))
                Questions = JsonConvert.DeserializeObject<List<QuestionInfo>>(File.ReadAllText(StoragePath));
            else
                Questions = new List<QuestionInfo>();
        }

        private static void SaveInternal() => File.WriteAllText(StoragePath, JsonConvert.SerializeObject(Questions));

        public static void AddQuestion(QuestionInfo question) {
            Questions.Add(question);
            SaveInternal();
        }

        public static void Clear() {
            Questions.Clear();
            SaveInternal();
        }
    }

    public class QuestionInfo
    {
        public DateTimeOffset DateTimeUTC { get; set; } = DateTimeOffset.UtcNow;

        public string Username { get; set; }

        public string QuestionText { get; set; }
    }

    [Group("question")]
    public class QuestionListModule : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [Summary("Adds a question to the list.")]
        [Alias("a")]
        public async Task AddQuestionAsync([Remainder] string questionText) {
            ListHandler.AddQuestion(new QuestionInfo {
                QuestionText = questionText,
                Username = Context.User.Username
            });

            await Context.Channel.SendMessageAsync("Question noted!").ConfigureAwait(false);
        }

        [Command("list")]
        [Summary("Lists the list of questions.")]
        [Alias("l")]
        public async Task ListQuestionsAsync() {
            var sb = new StringBuilder();

            foreach (var question in ListHandler.Questions) {
                sb.Append("On the ").Append(question.DateTimeUTC)
                    .Append(" \"").Append(question.Username)
                    .Append("\" asked \"").Append(question.QuestionText)
                    .Append("\"\n");
            }

            await Context.Channel.SendMessageAsync(sb.ToString()).ConfigureAwait(false);
        }

        [Command("clear")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        [RequireOwner]
        [Summary("Will completely clear the cache!")]
        public Task ClearQuestions() {
            ListHandler.Clear();
            return Task.CompletedTask;
        }
    }
}