namespace TelegramBot
{
    public class Word
    {
        public Word(string rusValue, string engValue, string wordTheme)
        {
            RusValue = rusValue;
            EngValue = engValue;
            WordTheme = wordTheme;
        }

        public string RusValue { get; }

        public string EngValue { get; }

        public string WordTheme { get; }
    }
}
