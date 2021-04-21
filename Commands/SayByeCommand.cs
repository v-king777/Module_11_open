namespace TelegramBot
{
    public class SayByeCommand : AbstractCommand, IChatTextCommand
    {
        public SayByeCommand()
        {
            CommandText = "/saymebye";
            CommandDescription = "сказать пока";
        }

        public string ReturnText()
        {
            return "Пока-пока!";
        }
    }
}
