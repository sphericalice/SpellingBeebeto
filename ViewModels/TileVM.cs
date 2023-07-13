using CommunityToolkit.Mvvm.Input;
using SpellingBeebeto.Models;
using SpellingBeebeto.Utilities;
using System.ComponentModel;
using System.Windows.Input;

namespace SpellingBeebeto.ViewModels
{
    public class TileVM : BindableBase
    {
        private readonly Tile Model;

        public char Letter => Model.Letter;
        public bool IsKeyTile => Model.IsKeyTile;
        public ICommand ClickTileCommand { get; }
        public TileVM(Tile model)
        {
            Model = model;

            ClickTileCommand = new RelayCommand(Model.TryAddTileToWord);
        }
    }
}
