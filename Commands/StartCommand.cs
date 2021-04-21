namespace TelegramBot
{
    public class StartCommand : AbstractCommand
    {
        public StartCommand()
        {
            CommandText = "/start";
            CommandDescription = "начать тренировку";
        }
    }
}
