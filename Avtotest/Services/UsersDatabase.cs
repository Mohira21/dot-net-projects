using Telegram.Bot.Types;
using User = Models.User;

namespace Services
{
    public class UsersDatabase
    {
        public List<User> Users = new List<User>();

        public User AddUser(long chatId, string name)
        {
            if(Users.Any(user => user.ChatId == chatId))
            {
                return Users.FirstOrDefault(u => u.ChatId == chatId)!;
            }
            
            var user = new User(chatId, name);
            Users.Add(user);
            return user;
        }

        public User? GetUser(long chatId)
        {
            return Users.FirstOrDefault(user => user.ChatId == chatId);
        }

        public string GetUsersText()
        {
            string usersText = "";

            for(int i = 0; i < Users.Count; i++)
            {
                usersText += Users[i].ToText() + "\n";
            }
            return usersText;
        }
    }
}