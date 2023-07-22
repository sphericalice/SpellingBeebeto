using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;
using static SpellingBeebeto.Models.GameElements.WordValidity;

namespace SpellingBeebeto.Models.GameElements;

public class GameBoard : BindableBase
{
    public RuleSet RuleSet { get; private set; }
    public Word Word { get; private set; }
    public Tile KeyTile { get; private set; }
    public IEnumerable<Tile> Tiles { get; private set; }
    public ObservableCollection<string> AcceptedWords { get; private set; }
    public Validity Validity => GetWordValidity();
    public GameBoard()
    {
        RuleSet = new RuleSet("collection");
        KeyTile = new Tile(this, RuleSet.KeyLetter, isKeyTile: true);
        Tiles = RuleSet.Letters.Select(letter => new Tile(this, letter));

        Word = new("");
        AcceptedWords = new ObservableCollection<string>();
    }

    internal bool IsWordEmpty() => Word.IsEmpty();
    internal bool IsWordTooShort() => Word.Text.Length < RuleSet.MinWordLength;
    internal bool IsWordTooLong() => Word.Text.Length >= RuleSet.MaxWordLength;
    internal bool IsWordMissingKeyLetter() => !Word.Text.Contains(RuleSet.KeyLetter);
    internal bool IsWordAlreadyFound() => AcceptedWords.Any(text => text.ToLower() == Word.Text.ToLower());
    internal bool IsWordNotInWordList() => !RuleSet.IsInWordList(Word.Text);
    internal Validity GetWordValidity()
    {
        if (IsWordEmpty()) return Validity.Empty;
        if (IsWordTooShort()) return Validity.TooShort;
        if (IsWordTooLong()) return Validity.TooLong;
        if (IsWordMissingKeyLetter()) return Validity.MissingKeyLetter;
        if (IsWordAlreadyFound()) return Validity.AlreadyFound;
        if (IsWordNotInWordList()) return Validity.NotInWordList;
        return Validity.Valid;
    }
    internal void AcceptWord()
    {
        AcceptedWords.Insert(0, Word.ToTitleCase());
        NotifyPropertyChanged(nameof(AcceptedWords));
        // TODO: Increment score for words in the word list
    }
    internal void SubmitWord()
    {
        if (Validity == Validity.Valid) AcceptWord();
        Word.Clear();
        NotifyPropertyChanged(nameof(Validity));
        NotifyPropertyChanged(nameof(Word));
    }

    internal void ShuffleTiles()
    {
        Tiles = Tiles.Shuffle();
        NotifyPropertyChanged(nameof(Tiles));
    }

    internal void AddLetterToWord(char letter)
    {
        Word.AddLetter(letter);
        NotifyPropertyChanged(nameof(Word));
    }

    internal void DeleteLastLetter()
    {
        Word.DeleteLastLetter();
        NotifyPropertyChanged(nameof(Word));
    }
}
