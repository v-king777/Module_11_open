namespace TelegramBot
{
    public abstract class AbstractCommand : IChatCommand
    {
        public string CommandText { get; set; }

        public string CommandDescription { get; set; }

        public bool CheckMessage(string message)
        {
            return CommandText == message;
        }
    }
}
