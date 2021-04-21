namespace TelegramBot
{
    public interface IChatTextCommand
    {
        /// <summary>
        /// Метод присваивает команде соответствующий текст
        /// </summary>
        /// <returns></returns>
        string ReturnText();
    }
}
