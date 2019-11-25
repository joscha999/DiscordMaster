using System;
using System.Collections.Generic;
using System.Text;

namespace DALModels
{
    public interface IUserStorage
    {
        BotUser GetUserByID(ulong id);

        BotUser GetUserByUsername(string username);

        IEnumerable<BotUser> GetUsers(Func<BotUser, bool> predicate);

        void AddOrUpdateUser(BotUser user);

        void RemoveUser(BotUser user);
    }
}