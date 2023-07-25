using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;
using WeCantSpell.Hunspell;
using static SpellingBeebeto.Models.GameElements.WordValidity;
using static SpellingBeebeto.Utilities.WordListHelper;
using Config = SpellingBeebeto.Models.GameConfiguration.GameConfiguration;

namespace SpellingBeebeto.Models.GameElements
{
    public class RuleSet : BindableBase
    {
        public readonly string Letters;
        public readonly char KeyLetter;
        public readonly WordList ValidWords;
        public int MinWordLength { get; private set; } = 4;
        public int MaxWordLength { get; private set; } = 12;
        public ObservableCollection<string> AcceptedWords { get; private set; }
        public RuleSet() : this("default") { }
        public RuleSet(string letters, string keyLetter, IEnumerable<string> acceptedWords)
        {
            letters = letters.ToUpper();
            keyLetter = keyLetter.ToUpper();
            KeyLetter = keyLetter.First();
            Letters = new string(letters.Where(letter => letter != KeyLetter).Distinct().ToArray());
            if (Letters.Length != Config.WordSize) throw new Exception("Incorrect amount of letters.");
            ValidWords = GetWordList(WordListLanguage.English);
            AcceptedWords = new(acceptedWords);
        }
        public RuleSet(string letters, string keyLetter) : this(letters, keyLetter, Array.Empty<string>()) { }
        public RuleSet(string letters) : this(letters, Config.Random.Next(0, Config.WordSize - 1)) { }
        public RuleSet(string letters, int i) : this(letters, letters[i]) { }
        public RuleSet(string letters, char i) : this(letters, i.ToString()) { }
        public RuleSet(string letters, string keyLetter, int minWordLength, int maxWordLength) : this(letters, keyLetter)
        {
            MinWordLength = minWordLength;
            MaxWordLength = maxWordLength;
        }

        internal static bool IsWordEmpty(string word) => word.Length == 0;
        internal bool IsWordTooShort(string word) => word.Length < MinWordLength;
        internal bool IsWordTooLong(string word) => word.Length >= MaxWordLength;
        internal bool IsWordMissingKeyLetter(string word) => !word.Contains(KeyLetter);
        internal bool IsWordAlreadyFound(string word) => AcceptedWords.Any(text => text.ToLower() == word.ToLower());
        internal bool IsWordNotInWordList(string word) => !ValidWords.CheckDetails(word.ToLower()).Correct;
        internal Validity GetWordValidity(Word word)
        {
            string text = word.Text;
            if (IsWordEmpty(text)) return Validity.Empty;
            if (IsWordTooShort(text)) return Validity.TooShort;
            if (IsWordTooLong(text)) return Validity.TooLong;
            if (IsWordMissingKeyLetter(text)) return Validity.MissingKeyLetter;
            if (IsWordAlreadyFound(text)) return Validity.AlreadyFound;
            if (IsWordNotInWordList(text)) return Validity.NotInWordList;
            return Validity.Valid;
        }
        internal void AcceptWord(Word word)
        {
            AcceptedWords.Insert(0, word.ToTitleCase());
            TrySave();
            NotifyPropertyChanged(nameof(AcceptedWords));
            // TODO: Increment score for words in the word list
        }
        public static RuleSet TryLoad()
        {
            string filePath = Config.SaveDataPath;
            try
            {
                if (File.Exists(filePath))
                {
                    using StreamReader reader = new(filePath);

                    string letters = reader.ReadLine() ?? throw new Exception("Expected letters.");
                    string keyLetter = reader.ReadLine() ?? throw new Exception("Expected key letter.");

                    if (!reader.EndOfStream)
                    {
                        string fileRemainder = reader.ReadToEnd();
                        if (fileRemainder is not null)
                        {
                            string[] acceptedWords = fileRemainder.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                            return new RuleSet(letters, keyLetter, acceptedWords);
                        }
                    }
                    return new RuleSet(letters, keyLetter);
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }
        public void TrySave()
        {
            string filePath = Config.SaveDataPath;
            try
            {
                using FileStream fileStream = new(filePath, File.Exists(filePath) ? FileMode.Truncate : FileMode.OpenOrCreate);
                using StreamWriter writer = new(fileStream);

                writer.WriteLine(Letters);
                writer.WriteLine(KeyLetter);
                foreach (string word in AcceptedWords) writer.WriteLine(word);
            }
            catch (Exception e)
            {

            }
        }
    }
}
