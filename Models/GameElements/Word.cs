using System.Globalization;

namespace SpellingBeebeto.Models.GameElements
{
    public class Word
    {
        public string Text { get; set; }
        public Word(string text)
        {
            Text = text;
        }
        internal void AddLetter(char letter)
        {
            Text += letter;
        }
        internal void DeleteLastLetter()
        {
            if (Text.Length < 1) return;
            Text = Text.Remove(Text.Length - 1, 1);
        }
        internal void Clear()
        {
            Text = "";
        }

        internal string ToTitleCase() => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Text.ToLower());

        internal bool IsEmpty() => Text.Length == 0;
    }
}
