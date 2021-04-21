namespace TelegramBot
{
    public class ListWordCommand : AbstractCommand
    {
        public ListWordCommand()
        {
            CommandText = "/listword";
            CommandDescription = "показать список добавленных слов";
        }
    }
}
