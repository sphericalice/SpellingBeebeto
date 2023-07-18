using SpellingBeebeto.Utilities;

namespace SpellingBeebeto.Models;

public class GameBoard : BindableBase
{
    public Word Word { get; private set; }
    private IEnumerable<string> WordList;
    private ICollection<string> SubmittedWords;
    public IEnumerable<Tile> Tiles { get; private set; }
    public GameBoard()
    {
        Word = new("");
        Tiles = new List<Tile>()
        {
            new Tile(this, 'H', isKeyTile: true),
            new Tile(this, 'A'),
            new Tile(this, 'R'),
            new Tile(this, 'O'),
            new Tile(this, 'U'),
            new Tile(this, 'N'),
            new Tile(this, 'E'),
        };
        WordList = new List<string>()
        {
            "RUNE",
            "NONE",
        };
        SubmittedWords = new HashSet<string>();
    }

    internal bool WordIsTooShort() => Word.Text.Length < Word.MinWordLength;
    internal bool WordIsTooLong() => Word.Text.Length >= Word.MaxWordLength;
    internal bool IsWordValid()
    {
        if (WordIsTooShort()) return false;
        if (WordIsTooLong()) return false;
        if (!WordList.Contains(Word.Text)) return false;
        return true;
    }

    internal void SubmitWord()
    {
        if (IsWordValid())
        {
            SubmittedWords.Add(Word.Text);
            // TODO: Increment score for words in the word list
        }
        Word.Text = "";
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
        if (SubmittedWords.Contains(Word.Text)) return false;
        return true;
    }
}
