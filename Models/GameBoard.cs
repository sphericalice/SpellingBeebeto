using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;

namespace SpellingBeebeto.Models;

public class GameBoard : BindableBase
{
    private readonly IEnumerable<string> WordList;
    public RuleSet RuleSet { get; private set; }
    public Word Word { get; private set; }
    public Tile KeyTile { get; private set; }
    public IEnumerable<Tile> Tiles { get; private set; }
    public ObservableCollection<string> AcceptedWords { get; private set; }
    public GameBoard()
    {
        Word = new("");
        KeyTile = new Tile(this, 'O', isKeyTile: true);
        Tiles = new List<Tile>()
        {
            new Tile(this, 'H'),
            new Tile(this, 'A'),
            new Tile(this, 'R'),
            new Tile(this, 'U'),
            new Tile(this, 'N'),
            new Tile(this, 'E'),
        };
        AcceptedWords = new ObservableCollection<string>() { "" };
        RuleSet = new RuleSet(KeyTile.Letter);
        WordList = new List<string>()
        {
            "RUNE",
            "NONE",
            "AURORA",
            "AURORAE",
            "EARN",
            "EARNER",
            "HEAR",
            "HEARER",
            "AREAR",
            "NEURON",
            "NEURONE",
            "NEON",
        };
    }

    internal bool WordIsTooShort() => Word.Text.Length < RuleSet.MinWordLength;
    internal bool WordIsTooLong() => Word.Text.Length >= RuleSet.MaxWordLength;
    internal bool IsWordValid()
    {
        if (WordIsTooShort()) return false;
        if (WordIsTooLong()) return false;
        if (!Word.Text.Contains(RuleSet.KeyLetter)) return false;
        if (!WordList.Contains(Word.Text)) return false;
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
}
