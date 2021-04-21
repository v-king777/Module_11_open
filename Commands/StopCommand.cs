namespace TelegramBot
{
    public class StopCommand : AbstractCommand
    {
        public StopCommand()
        {
            CommandText = "/stop";
            CommandDescription = "остановить тренировку";
        }
    }
}
