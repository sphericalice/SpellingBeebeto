using CommunityToolkit.Mvvm.Input;
using SpellingBeebeto.Models.GameElements;
using SpellingBeebeto.Utilities;
using System.Windows.Input;

namespace SpellingBeebeto.ViewModels.GameElements
{
    public class TileVM : BindableBase
    {
        private readonly Tile Model;
        public GameBoardVM GameBoard { get; set; }
        public char Letter => Model.Letter;
        public bool IsKeyTile => Model.IsKeyTile;
        public ICommand ClickTileCommand { get; }
        public TileVM(Tile model)
        {
            Model = model;

            ClickTileCommand = new RelayCommand(TryAddTileLetterToWord);
        }

        private void TryAddTileLetterToWord()
        {
            if (GameBoard.CanClickTiles()) Model.AddTileLetterToWord();
        }
    }
}
