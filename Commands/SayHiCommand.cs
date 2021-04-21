namespace TelegramBot
{
    public class SayHiCommand : AbstractCommand, IChatTextCommand
    {
        public SayHiCommand()
        {
            CommandText = "/saymehi";
            CommandDescription = "сказать привет";
        }

        public string ReturnText()
        {
            return "Привет!";
        }
    }
}
