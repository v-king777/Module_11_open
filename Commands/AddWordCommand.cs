namespace TelegramBot
{
    public class AddWordCommand : AbstractCommand
    {
        public AddWordCommand()
        {
            CommandText = "/addword";
            CommandDescription = "добавить в словарь новое слово";
        }
    }
}
