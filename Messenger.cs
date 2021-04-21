using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot
{
    public class Messenger
    {
        private ITelegramBotClient botClient;
        private CommandParser parser;
        private Dictionary<long, UserDictionary> userDictionaryList;

        public Messenger(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            parser = new CommandParser();
            userDictionaryList = new Dictionary<long, UserDictionary>();

            RegisterCommands();
        }

        /// <summary>
        /// Метод проверяет по Id чата, существует ли словарь.
        /// Если нет, то создаёт новый и добавляет его в коллекцию
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public async Task InitUserDictionary(Conversation chat)
        {
            var chatId = chat.GetId();

            if (!userDictionaryList.ContainsKey(chatId))
            {
                var newUserDictionary = new UserDictionary();
                userDictionaryList.Add(chatId, newUserDictionary);
            }

            await MakeAnswer(chat, chatId);
        }

        /// <summary>
        /// Метод выполняет проверку много чего, и это не есть хорошо
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task MakeAnswer(Conversation chat, long chatId)
        {
            var lastMessage = chat.GetLastMessage();

            if (lastMessage == "/stop" && chat.ChatOperation == "CheckWord")
            {
                chat.IsCommandMode = true;
                chat.ChatOperation = string.Empty;

                var text = "Тренировка завершена";
                await SendText(chat, text);
            }
            else if (parser.IsMessageCommand(lastMessage) && chat.IsCommandMode == true)
            {
                await ExecCommand(chat, chatId, lastMessage);
            }
            else if (chat.IsCommandMode == true)
            {
                var text = "Неизвестная команда,\nвведите /help";
                await SendText(chat, text);
            }
            else if (chat.IsCommandMode == false)
            {
                await ExecOperation(chat, chatId, lastMessage);
            }
        }

        /// <summary>
        /// Метод выполняет выбранную команду
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task ExecCommand(Conversation chat, long chatId, string command)
        {
            if (parser.IsTextCommand(command))
            {
                var text = parser.GetMessageText(command);
                await SendText(chat, text);
            }

            if (parser.IsAddWordCommand(command))
            {
                chat.IsCommandMode = false;
                await EnterRusValue(chat);
            }

            if (parser.IsDelWordCommand(command))
            {
                chat.IsCommandMode = false;
                await EnterDelWord(chat, chatId);
            }

            if (parser.IsClearCommand(command))
            {
                chat.IsCommandMode = false;
                await СonfirmСleaning(chat, chatId);
            }

            if (parser.IsListWordCommand(command))
            {
                await ShowDictionary(chat, chatId);
            }

            if (parser.IsStartCommand(command))
            {
                chat.IsCommandMode = false;
                await EnterTrainingMode(chat, chatId);
            }

            if (parser.IsStopCommand(command))
            {
                var text = "Тренировка ещё не началась";
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод выполняет выбранную операцию
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task ExecOperation(Conversation chat, long chatId, string chatMessage)
        {
            switch (chat.ChatOperation)
            {
                case "AddRusValue":
                    await AddRusValue(chat, chatId, chatMessage);
                    break;
                case "AddEngValue":
                    await AddEngValue(chat, chatId, chatMessage);
                    break;
                case "AddWordTheme":
                    await AddWordTheme(chat, chatId, chatMessage);
                    break;
                case "DelWord":
                    await ExecDelWord(chat, chatId, chatMessage);
                    break;
                case "ClearUserDictionary":
                    await ExecClearUserDictionary(chat, chatId, chatMessage);
                    break;
                case "StartTraining":
                    await StartTraining(chat, chatId, chatMessage);
                    break;
                case "CheckWord":
                    await ExecCheckWord(chat, chatId, chatMessage);
                    break;
            }
        }

        /// <summary>
        /// Метод запрашивает ввод русского значения
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        private async Task EnterRusValue(Conversation chat)
        {
            chat.ChatOperation = "AddRusValue";

            var text = "Введите русское значение";
            await SendText(chat, text);
        }

        /// <summary>
        /// Метод добавляет русское значение в словарь, если таковое отсутствует
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task AddRusValue(Conversation chat, long chatId, string chatMessage)
        {
            var userDictionary = userDictionaryList[chatId];

            if (!userDictionary.IsWordExists(chatMessage))
            {
                userDictionary.RusValue = chatMessage.ToLower();
                await EnterEngValue(chat);
            }
            else
            {
                chat.IsCommandMode = true;

                var text = $"Слово '{chatMessage}' уже есть в словаре";
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод запрашивает ввод английского значения
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        private async Task EnterEngValue(Conversation chat)
        {
            chat.ChatOperation = "AddEngValue";

            var text = "Введите английское значение";
            await SendText(chat, text);
        }

        /// <summary>
        /// Метод добавляет английское значение в словарь
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task AddEngValue(Conversation chat, long chatId, string chatMessage)
        {
            var userDictionary = userDictionaryList[chatId];
            userDictionary.EngValue = chatMessage.ToLower();
            await EnterWordTheme(chat);
        }

        /// <summary>
        /// Метод запрашивает ввод тематики
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        private async Task EnterWordTheme(Conversation chat)
        {
            chat.ChatOperation = "AddWordTheme";

            var text = "Введите тематику";
            await SendText(chat, text);
        }

        /// <summary>
        /// Метод добавляет тематику в словарь
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task AddWordTheme(Conversation chat, long chatId, string chatMessage)
        {
            var userDictionary = userDictionaryList[chatId];
            userDictionary.WordTheme = chatMessage.ToLower();
            await ExecAddWord(chat, chatId);
        }

        /// <summary>
        /// Метод добавляет слово в словарь и завершает операцию
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task ExecAddWord(Conversation chat, long chatId)
        {
            chat.IsCommandMode = true;

            var userDictionary = userDictionaryList[chatId];
            userDictionary.AddWord();

            var text = $"Слово '{userDictionary.RusValue}' успешно добавлено в словарь";
            await SendText(chat, text);
        }

        /// <summary>
        /// Метод запрашивает ввод слова для удаления, если словарь не пустой
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task EnterDelWord(Conversation chat, long chatId)
        {
            var userDictionary = userDictionaryList[chatId];

            if (userDictionary.IsEmpty())
            {
                chat.IsCommandMode = true;

                var text = "Словарь пуст";
                await SendText(chat, text);
            }
            else
            {
                chat.ChatOperation = "DelWord";

                var text = "Какое слово хотите удалить?\nВведите русское значение";
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод выполняет команду удаления слова, если оно найдено в словаре
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task ExecDelWord(Conversation chat, long chatId, string chatMessage)
        {
            chat.IsCommandMode = true;

            var userDictionary = userDictionaryList[chatId];

            if (userDictionary.IsWordExists(chatMessage))
            {
                userDictionary.DelWord(chatMessage);

                var text = $"Слово '{chatMessage}' успешно удалено";
                await SendText(chat, text);
            }
            else
            {
                var text = $"Слово '{chatMessage}' не найдено в словаре";
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод запрашивает подтверждение на очистку словаря, если словарь не пустой
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task СonfirmСleaning(Conversation chat, long chatId)
        {
            var userDictionary = userDictionaryList[chatId];

            if (userDictionary.IsEmpty())
            {
                chat.IsCommandMode = true;

                var text = "Словарь и так чище некуда";
                await SendText(chat, text);
            }
            else
            {
                chat.ChatOperation = "ClearUserDictionary";

                var text = "Все данные из словаря будут удалены!\n/yes - подтвердить";
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод выполняет команду очистки словаря
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task ExecClearUserDictionary(Conversation chat, long chatId, string chatMessage)
        {
            chat.IsCommandMode = true;

            var userDictionary = userDictionaryList[chatId];

            if (chatMessage == "/yes")
            {
                userDictionary.ClearUserDictionary();

                var text = "Ок, словарь очищен";
                await SendText(chat, text);
            }
            else
            {
                var text = "Очистка отменена";
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод показывает список добавленных слов, если словарь не пустой
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task ShowDictionary(Conversation chat, long chatId)
        {
            var userDictionary = userDictionaryList[chatId];

            if (userDictionary.IsEmpty())
            {
                var text = "Словарь пуст";
                await SendText(chat, text);
            }
            else
            {
                var text = userDictionary.GetWordList();
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод запрашивает режим тренировки, если словарь не пустой
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task EnterTrainingMode(Conversation chat, long chatId)
        {
            var userDictionary = userDictionaryList[chatId];

            if (userDictionary.IsEmpty())
            {
                chat.IsCommandMode = true;

                var text = "Чтобы начать тренировку, нужно сначала пополнить словарный запас";
                await SendText(chat, text);
            }
            else
            {
                chat.ChatOperation = "StartTraining";

                var text = "Выберите режим тренировки:\n" +
                    "/rustoeng - с русского на английский\n" +
                    "/engtorus - с английского на русский\n" +
                    "/cancel - отмена";
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод запускает тренировку
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task StartTraining(Conversation chat, long chatId, string chatMessage)
        {
            chat.ChatOperation = "CheckWord";

            var userDictionary = userDictionaryList[chatId];

            switch (chatMessage)
            {
                case "/rustoeng":
                    {
                        chat.TrainingMode = "RusToEng";

                        var word = userDictionary.GetTrainingWord(chat.TrainingMode);

                        var text = "Тренировка началась!\nСлово: " + word;
                        await SendText(chat, text);
                        break;
                    }

                case "/engtorus":
                    {
                        chat.TrainingMode = "EngToRus";

                        var word = userDictionary.GetTrainingWord(chat.TrainingMode);

                        var text = "Тренировка началась!\nСлово: " + word;
                        await SendText(chat, text);
                        break;
                    }

                case "/cancel":
                    {
                        chat.IsCommandMode = true;

                        var text = "Тренировка отменена";
                        await SendText(chat, text);
                        break;
                    }

                default:
                    await EnterTrainingMode(chat, chatId);
                    break;
            }
        }

        /// <summary>
        /// Метод проверяет введённое слово, выдаёт результат проверки и генерирует следующее слово
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        private async Task ExecCheckWord(Conversation chat, long chatId, string chatMessage)
        {
            var userDictionary = userDictionaryList[chatId];

            if (userDictionary.CheckWord(chat.TrainingMode, userDictionary.TrainingWord, chatMessage))
            {
                var word = userDictionary.GetTrainingWord(chat.TrainingMode);

                var text = "Верно!\nСледующее слово: " + word;
                await SendText(chat, text);
            }
            else
            {
                var word = userDictionary.GetTrainingWord(chat.TrainingMode);

                var text = "Неверно!\nСледующее слово: " + word;
                await SendText(chat, text);
            }
        }

        /// <summary>
        /// Метод регистрирует новые команды
        /// </summary>
        private void RegisterCommands()
        {
            parser.AddCommand(new SayHiCommand());
            parser.AddCommand(new SayByeCommand());
            parser.AddCommand(new HelpCommand());
            parser.AddCommand(new AddWordCommand());
            parser.AddCommand(new DelWordCommand());
            parser.AddCommand(new ListWordCommand());
            parser.AddCommand(new ClearCommand());
            parser.AddCommand(new StartCommand());
            parser.AddCommand(new StopCommand());
        }

        /// <summary>
        /// Метод отправляет сообщение в чат
        /// </summary>
        /// <param name="chat"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private async Task SendText(Conversation chat, string text)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }
    }
}
