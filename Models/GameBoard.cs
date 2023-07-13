using SpellingBeebeto.Utilities;

namespace SpellingBeebeto.Models;

public class GameBoard : BindableBase
{
    private readonly int MinWordLength = 4;
    private readonly int MaxWordLength = 12;
    private IEnumerable<string> WordList;
    public string Word { get; private set; }
    public IEnumerable<Tile> Tiles { get; private set; }
    public GameBoard()
    {
        Word = "";
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
    }

    internal void AddLetterToWord(char letter)
    {
        if (Word.Length >= MaxWordLength)
        {
            RejectWord();
            return;
        }
        Word += letter;
        NotifyPropertyChanged(nameof(Word));
    }

    private void RejectWord()
    {
        Word = "";
        NotifyPropertyChanged(nameof(Word));
    }

    internal void DeleteLastLetter()
    {
        if (Word.Length > 1) Word = Word.Remove(Word.Length - 1, 1);
        NotifyPropertyChanged(nameof(Word));
    }

    internal void ShuffleTiles()
    {
        Tiles = Tiles.Shuffle();
        NotifyPropertyChanged(nameof(Tiles));
    }

    internal void SubmitWord()
    {
        if (Word.Length < MinWordLength || !WordList.Contains(Word))
        {
            RejectWord();
            return;
        }
        // TODO: Increment score for words in the word list
        Word = "";
        NotifyPropertyChanged(nameof(Word));
    }
}
