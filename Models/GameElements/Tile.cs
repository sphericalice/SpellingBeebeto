using SpellingBeebeto.Utilities;

namespace SpellingBeebeto.Models.GameElements
{
    public class Tile : BindableBase
    {
        public GameBoard GameBoard { get; }
        public char Letter { get; }
        public bool IsKeyTile { get; }
        public Tile(GameBoard gameBoard, char letter, bool isKeyTile = false)
        {
            GameBoard = gameBoard;
            Letter = letter;
            IsKeyTile = isKeyTile;
        }

        internal void AddTileLetterToWord()
        {
            GameBoard.AddLetterToWord(Letter);
        }
    }
}
