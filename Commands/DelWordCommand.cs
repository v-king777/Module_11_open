namespace TelegramBot
{
    public class DelWordCommand : AbstractCommand
    {
        public DelWordCommand()
        {
            CommandText = "/delword";
            CommandDescription = "удалить слово из словаря";
        }
    }
}
