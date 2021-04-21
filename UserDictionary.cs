using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelegramBot
{
    public class UserDictionary
    {
        private List<Word> words;

        public UserDictionary()
        {
            words = new List<Word>();
        }

        public string RusValue { get; set; }

        public string EngValue { get; set; }

        public string WordTheme { get; set; }

        public string TrainingWord { get; set; }

        /// <summary>
        /// Метод добавляет в словарь новое слово
        /// </summary>
        public void AddWord()
        {
            words.Add(new Word(RusValue, EngValue, WordTheme));
        }

        /// <summary>
        /// Метод удаляет из словаря введённое слово
        /// </summary>
        /// <param name="text"></param>
        public void DelWord(string text)
        {
            var word = words.Find(x => x.RusValue.Contains(text.ToLower()));
            words.Remove(word);
        }

        /// <summary>
        /// Метод очищает словарь
        /// </summary>
        public void ClearUserDictionary()
        {
            words.Clear();
        }

        /// <summary>
        /// Метод проверяет наличие слова в словаре
        /// </summary>
        /// <param name="rusValue"></param>
        /// <returns></returns>
        public bool IsWordExists(string text)
        {
            return words.Exists(x => x.RusValue == text.ToLower());
        }

        /// <summary>
        /// Метод проверяет, пустой ли словарь
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return words.Count == 0;
        }

        /// <summary>
        /// Метод возвращает список добавленных слов
        /// </summary>
        /// <returns></returns>
        public string GetWordList()
        {
            var sb = new StringBuilder();
            var text = string.Empty;

            foreach (var word in words)
            {
                text = "Добавленные слова:\n" + sb.Append($"{word.RusValue}\n").ToString();
            }

            return text;
        }

        /// <summary>
        /// Метод возвращает рандомное слово из словаря в зависимости от выбранного режима
        /// </summary>
        /// <param name="trainingMode"></param>
        /// <returns></returns>
        public string GetTrainingWord(string trainingMode)
        {
            var random = new Random();
            var item = random.Next(0, words.Count);
            var randomWord = words.AsEnumerable().ElementAt(item);

            switch (trainingMode)
            {
                case "RusToEng":
                    TrainingWord = randomWord.RusValue;
                    break;

                case "EngToRus":
                    TrainingWord = randomWord.EngValue;
                    break;
            }

            return TrainingWord;
        }

        /// <summary>
        /// Метод возвращает результат проверки слова на правильный перевод
        /// </summary>
        /// <param name="trainingMode"></param>
        /// <param name="word"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public bool CheckWord(string trainingMode, string word, string answer)
        {
            Word control;
            var result = false;

            switch (trainingMode)
            {
                case "RusToEng":
                    {
                        control = words.FirstOrDefault(x => x.RusValue == word);
                        result = control.EngValue == answer.ToLower();
                        break;
                    }

                case "EngToRus":
                    {
                        control = words.FirstOrDefault(x => x.EngValue == word);
                        result = control.RusValue == answer.ToLower();
                        break;
                    }
            }

            return result;
        }
    }
}
