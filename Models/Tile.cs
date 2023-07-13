﻿using SpellingBeebeto.Utilities;

namespace SpellingBeebeto.Models
{
    public class Tile : BindableBase
    {
        public GameBoard GameBoard { get; }
        public char Letter { get; }
        public bool IsKeyTile { get; }
        public Tile(GameBoard gameBoard, char letter, bool isKeyTile=false)
        {
            GameBoard = gameBoard;
            Letter = letter;
            IsKeyTile = isKeyTile;
        }

        public void TryAddTileToWord()
        {
            GameBoard.AddLetterToWord(Letter);
        }
    }
}
