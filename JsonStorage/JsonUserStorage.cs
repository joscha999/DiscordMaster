using DALModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace JsonStorage
{
    public class JsonUserStorage : IUserStorage
    {
        //cache
        private readonly List<BotUser> users;

        public string Path { get; }

        public JsonUserStorage(string path) {
            if (File.Exists(path))
                users = JsonConvert.DeserializeObject<List<BotUser>>(File.ReadAllText(path));
            else
                users = new List<BotUser>();

            Path = path;
        }

        private void InternalSave() => File.WriteAllText(Path, JsonConvert.SerializeObject(users));

        public void AddOrUpdateUser(BotUser user) {
            var cUser = users.Find(u => u.DiscordID == user.DiscordID);

            if (cUser != null) {
                //found => update
                cUser.Update(user);
            } else {
                //not found => add
                users.Add(user);
            }

            InternalSave();
        }

        public void RemoveUser(BotUser user) {
            users.RemoveAll(u => u.DiscordID == user.DiscordID);
            InternalSave();
        }

        public BotUser GetUserByID(ulong id) => users.Find(u => u.DiscordID == id);

        public BotUser GetUserByUsername(string username) => users.Find(u => u.Username == username);

        public IEnumerable<BotUser> GetUsers(Func<BotUser, bool> predicate) => users.FindAll(u => predicate(u));
    }
}