using System;

namespace TelegramBot
{
    /// <summary>
    /// Основной класс
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Основной метод - точка входа в программу
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            var bot = new BotWorker();

            try
            {
                bot.Initialize();
                bot.Start();

                Console.WriteLine("Напишите 'stop' для прекращения работы");

                string command;
                do
                {
                    command = Console.ReadLine();
                }
                while (command != "stop");

                bot.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
