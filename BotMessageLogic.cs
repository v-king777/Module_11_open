using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    public class BotMessageLogic
    {
        private Messenger messenger;
        private Dictionary<long, Conversation> chatList;

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            messenger = new Messenger(botClient);
            chatList = new Dictionary<long, Conversation>();
        }

        /// <summary>
        /// Метод проверяет по Id, существует ли чат.
        /// Если нет, то создаёт новый и добавляет его в коллекцию.
        /// Далее добавляет полученное сообщениее в список конкретного чата
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task Response(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;

            if (!chatList.ContainsKey(id))
            {
                var newChat = new Conversation(e.Message.Chat);
                chatList.Add(id, newChat);
            }

            var chat = chatList[id];
            chat.AddMessage(e.Message);
            await MessageHandling(chat);
        }

        /// <summary>
        /// Метод начинает обработку полученного сообщения
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        private async Task MessageHandling(Conversation chat)
        {
            await messenger.InitUserDictionary(chat);
        }
    }
}
