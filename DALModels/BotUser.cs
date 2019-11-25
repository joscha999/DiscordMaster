using System;
using System.Collections.Generic;
using System.Text;

namespace DALModels
{
    public class BotUser
    {
        public ulong DiscordID { get; set; }

        public string Username { get; set; }

        public Dictionary<string, object> Preferences { get; set; } = new Dictionary<string, object>();

        public bool TryGetPreference<T>(string key, out T preference) {
            if (Preferences.TryGetValue(key, out var value) && value is T tValue) {
                preference = tValue;
                return true;
            }

            preference = default;
            return false;
        }

        public void Update(BotUser user) {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (DiscordID != user.DiscordID)
                throw new ArgumentException("Cannot update a user from a different user!");

            Username = user.Username;

            Preferences = user.Preferences;
        }
    }
}