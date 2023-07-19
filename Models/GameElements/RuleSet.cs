namespace SpellingBeebeto.Models.GameElements
{
    public class RuleSet
    {
        public readonly string Letters;
        public readonly char KeyLetter;
        public readonly HashSet<string> WordList;
        public int MinWordLength { get; private set; } = 4;
        public int MaxWordLength { get; private set; } = 12;
        public RuleSet(string letters, char keyLetter)
        {
            KeyLetter = keyLetter;
            Letters = new string(letters.Where(letter => letter != KeyLetter).ToArray());
            if (Letters.Length != 6) throw new Exception("Incorrect amount of letters.");
            WordList = SetupWordList().ToHashSet();
        }
        public RuleSet(string letters) : this(letters, letters[0]) { }
        public RuleSet(string letters, int i) : this(letters, letters[i]) { }
        public RuleSet(string letters, char keyLetter, int minWordLength, int maxWordLength) : this(letters, keyLetter)
        {
            MinWordLength = minWordLength;
            MaxWordLength = maxWordLength;
        }
        private IEnumerable<string> SetupWordList()
        {
            // TODO: Correctly generate word list from input letters
            yield return "RUNE";
            yield return "NONE";
            yield return "AURORA";
            yield return "AURORAE";
            yield return "EARN";
            yield return "EARNER";
            yield return "HEAR";
            yield return "HONE";
            yield return "HEARER";
            yield return "AREAR";
            yield return "NEURON";
            yield return "NEURONE";
            yield return "NEON";
        }
    }
}
