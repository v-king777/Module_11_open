using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TelegramBot
{
    public class Conversation
    {
        private Chat telegramChat;
        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
        }

        public bool IsCommandMode { get; set; } = true;

        public string ChatOperation { get; set; }

        public string TrainingMode { get; set; }

        /// <summary>
        /// Метод добавляет полученное сообщение в список
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        /// <summary>
        /// Метод возвращает Id чата
        /// </summary>
        /// <returns></returns>
        public long GetId()
        {
            return telegramChat.Id;
        }

        /// <summary>
        /// Метод возвращает последнее сообщение чата
        /// </summary>
        /// <returns></returns>
        public string GetLastMessage()
        {
            return telegramMessages[telegramMessages.Count - 1].Text;
        }
    }
}
