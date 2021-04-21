namespace TelegramBot
{
    public interface IChatCommand
    {
        public string CommandText { get; set; }

        public string CommandDescription { get; set; }

        /// <summary>
        /// Метод проверяет сообщение на наличие в списке команд
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool CheckMessage(string message);
    }
}
