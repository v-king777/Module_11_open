using System.Collections.Generic;
using System.Text;

namespace TelegramBot
{
    public class CommandParser
    {
        private static List<IChatCommand> commands;

        public CommandParser()
        {
            commands = new List<IChatCommand>();
        }

        /// <summary>
        /// Метод возвращает список доступных команд с их описанием
        /// </summary>
        /// <returns></returns>
        public static string GetListCommands()
        {
            var sb = new StringBuilder();
            var s = string.Empty;

            foreach (var command in commands)
            {
                s = sb.Append($"{command.CommandText} - {command.CommandDescription}\n").ToString();
            }

            return s;
        }

        /// <summary>
        /// Метод добавляет команду в список
        /// </summary>
        /// <param name="chatCommand"></param>
        public void AddCommand(IChatCommand chatCommand)
        {
            commands.Add(chatCommand);
        }

        /// <summary>
        /// Метод проверяет, является ли введённое сообщение командой
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsMessageCommand(string message)
        {
            return commands.Exists(x => x.CheckMessage(message));
        }

        /// <summary>
        /// Метод проверяет, является ли команда текстовой
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsTextCommand(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message));
            return command is IChatTextCommand;
        }

        /// <summary>
        /// Метод проверяет, является ли команда AddWordCommand
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsAddWordCommand(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message));
            return command is AddWordCommand;
        }

        /// <summary>
        /// Метод проверяет, является ли команда DelWordCommand
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsDelWordCommand(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message));
            return command is DelWordCommand;
        }

        /// <summary>
        /// Метод проверяет, является ли команда ListWordCommand
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsListWordCommand(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message));
            return command is ListWordCommand;
        }

        /// <summary>
        /// Метод проверяет, является ли команда ClearCommand
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsClearCommand(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message));
            return command is ClearCommand;
        }

        /// <summary>
        /// Метод проверяет, является ли команда StartCommand
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsStartCommand(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message));
            return command is StartCommand;
        }

        /// <summary>
        /// Метод проверяет, является ли команда StopCommand
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsStopCommand(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message));
            return command is StopCommand;
        }

        /// <summary>
        /// Метод возвращает присвоенный команде текст
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string GetMessageText(string message)
        {
            var command = commands.Find(x => x.CheckMessage(message)) as IChatTextCommand;
            return command.ReturnText();
        }
    }
}
