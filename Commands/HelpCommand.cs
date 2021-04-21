namespace TelegramBot
{
    public class HelpCommand : AbstractCommand, IChatTextCommand
    {
        public HelpCommand()
        {
            CommandText = "/help";
            CommandDescription = "показать список команд";
        }

        public string ReturnText()
        {
            var сommandList = CommandParser.GetListCommands();
            return "Список доступных команд:\n" + сommandList;
        }
    }
}
