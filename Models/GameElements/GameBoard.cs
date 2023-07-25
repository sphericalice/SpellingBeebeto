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
    public Validity Validity => RuleSet.GetWordValidity(Word);
    public ObservableCollection<string> AcceptedWords => RuleSet.AcceptedWords;
    public GameBoard(RuleSet ruleSet)
    {
        RuleSet = ruleSet;
        KeyTile = new Tile(this, RuleSet.KeyLetter, isKeyTile: true);
        Tiles = RuleSet.Letters.Select(letter => new Tile(this, letter));

        Word = new("");
        NotifyPropertyChanged(nameof(AcceptedWords));
    }
    internal void SubmitWord()
    {
        if (Validity == Validity.Valid) RuleSet.AcceptWord(Word);
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
    internal Validity GetWordValidity() => RuleSet.GetWordValidity(Word);
    internal bool IsWordTooLong() => RuleSet.IsWordTooLong(Word.Text);
}
