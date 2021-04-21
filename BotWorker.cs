using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    /// <summary>
    /// Класс отвечает за работу телеграмм-бота на верхнем уровне
    /// </summary>
    public class BotWorker
    {
        private ITelegramBotClient botClient;
        private BotMessageLogic logic;

        /// <summary>
        /// Метод создаёт клиент телеграмм-бота
        /// </summary>
        public void Initialize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic(botClient);
        }

        /// <summary>
        /// Метод устанавливает событие для отправки сообщений и начинает ожидание этих сообщений
        /// </summary>
        public void Start()
        {
            botClient.OnMessage += BotOnMessage;
            botClient.StartReceiving();
        }

        /// <summary>
        /// Метод завершает процесс ожидания сообщений
        /// </summary>
        public void Stop()
        {
            botClient.StopReceiving();
        }

        /// <summary>
        /// Метод выполняется при получении сообщений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BotOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await logic.Response(e);
            }
        }
    }
}
