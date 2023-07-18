namespace SpellingBeebeto.Models
{
    public class Word
    {
        public readonly int MinWordLength = 4;
        public readonly int MaxWordLength = 12;
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
    }
}
