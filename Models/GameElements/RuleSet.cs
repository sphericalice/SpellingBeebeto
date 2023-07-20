using WeCantSpell.Hunspell;
using static SpellingBeebeto.Utilities.WordListHelper;
using Config = SpellingBeebeto.Models.GameConfiguration.GameConfiguration;

namespace SpellingBeebeto.Models.GameElements
{
    public class RuleSet
    {
        public readonly string Letters;
        public readonly char KeyLetter;
        public readonly WordList ValidWords;
        public int MinWordLength { get; private set; } = 4;
        public int MaxWordLength { get; private set; } = 12;

        public RuleSet(string letters, string keyLetter)
        {
            letters = letters.ToUpper();
            keyLetter = keyLetter.ToUpper();
            KeyLetter = keyLetter.First();
            Letters = new string(letters.Where(letter => letter != KeyLetter).Distinct().ToArray());
            if (Letters.Length != Config.WordSize) throw new Exception("Incorrect amount of letters.");
            ValidWords = GetWordList(WordListLanguage.English);
        }
        public RuleSet(string letters) : this(letters, Config.Random.Next(0, Config.WordSize - 1)) { }
        public RuleSet(string letters, int i) : this(letters, letters[i]) { }
        public RuleSet(string letters, char i) : this(letters, i.ToString()) { }
        public RuleSet(string letters, string keyLetter, int minWordLength, int maxWordLength) : this(letters, keyLetter)
        {
            MinWordLength = minWordLength;
            MaxWordLength = maxWordLength;
        }

        internal bool IsInWordList(string word) => ValidWords.CheckDetails(word.ToLower()).Correct;
    }
}
