namespace TelegramBot
{
    public class ClearCommand : AbstractCommand
    {
        public ClearCommand()
        {
            CommandText = "/clear";
            CommandDescription = "очистить словарь";
        }
    }
}
