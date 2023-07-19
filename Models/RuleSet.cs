namespace SpellingBeebeto.Models
{
    public class RuleSet
    {
        public readonly char KeyLetter = ' ';
        public int MinWordLength { get; private set; } = 4;
        public int MaxWordLength { get; private set; } = 12;
        public RuleSet(char keyLetter)
        {
            KeyLetter = keyLetter;
        }
        public RuleSet(char keyLetter, int minWordLength, int maxWordLength)
        {
            KeyLetter = keyLetter;
            MinWordLength = minWordLength;
            MaxWordLength = maxWordLength;
        }
    }
}
