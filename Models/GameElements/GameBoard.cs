using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;

namespace SpellingBeebeto.Models.GameElements;

public class GameBoard : BindableBase
{
    private readonly Random random = new();
    public RuleSet RuleSet { get; private set; }
    public Word Word { get; private set; }
    public Tile KeyTile { get; private set; }
    public IEnumerable<Tile> Tiles { get; private set; }
    public ObservableCollection<string> AcceptedWords { get; private set; }
    public GameBoard()
    {
        RuleSet = new RuleSet("HARUNEO", random.Next(0, Configuration.Configuration.WordSize - 1));
        KeyTile = new Tile(this, RuleSet.KeyLetter, isKeyTile: true);
        Tiles = RuleSet.Letters.Select(letter => new Tile(this, letter));

        Word = new("");
        AcceptedWords = new ObservableCollection<string>() { "" };
    }

    internal bool WordIsTooShort() => Word.Text.Length < RuleSet.MinWordLength;
    internal bool WordIsTooLong() => Word.Text.Length >= RuleSet.MaxWordLength;
    internal bool IsWordValid()
    {
        if (WordIsTooShort()) return false;
        if (WordIsTooLong()) return false;
        if (!Word.Text.Contains(RuleSet.KeyLetter)) return false;
        if (!RuleSet.WordList.Contains(Word.Text)) return false;
        return true;
    }

    internal void SubmitWord()
    {
        if (IsWordValid())
        {
            AcceptedWords.Insert(0, Word.AsTitleCase());
            NotifyPropertyChanged(nameof(AcceptedWords));
            // TODO: Increment score for words in the word list
        }
        Word.Clear();
        NotifyPropertyChanged(nameof(Word));
    }

    internal void ShuffleTiles()
    {
        Tiles = Tiles.Shuffle();
        NotifyPropertyChanged(nameof(Tiles));
    }

    internal void TryAddTileToWord(Tile tile)
    {
        if (WordIsTooLong()) return;
        Word.AddLetter(tile.Letter);
        NotifyPropertyChanged(nameof(Word));
    }

    internal void DeleteLastLetter()
    {
        Word.DeleteLastLetter();
        NotifyPropertyChanged(nameof(Word));
    }

    internal bool CanSubmitWord()
    {
        if (!IsWordValid()) return false;
        if (AcceptedWords.Any(text => text.ToLower() == Word.Text.ToLower())) return false;
        return true;
    }

    internal bool WordIsEmpty() => Word.IsEmpty();
}
