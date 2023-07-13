using CommunityToolkit.Mvvm.Input;
using SpellingBeebeto.Models;
using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SpellingBeebeto.ViewModels;

public class GameBoardVM : BindableBase
{
    private readonly GameBoard Model;

    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string Message => "Welcome to Spelling Beebeto";
    public string Word => Model.Word;
    public ObservableCollection<TileVM> Tiles { get; private set; }
    public IRelayCommand DeleteLetterCommand { get; }
    public IRelayCommand ShuffleTilesCommand { get; }
    public IRelayCommand SubmitWordCommand { get; }

    public GameBoardVM(GameBoard model)
    {
        Model = model;
        Tiles = new(Model.Tiles.Select(tile => new TileVM(tile)));
        Model.PropertyChanged += UpdateVM;

        DeleteLetterCommand = new RelayCommand(Model.DeleteLastLetter);
        ShuffleTilesCommand = new RelayCommand(Model.ShuffleTiles);
        SubmitWordCommand = new RelayCommand(Model.SubmitWord);
    }

    private void UpdateVM(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Tiles)) Tiles = new(Model.Tiles.Select(tile => new TileVM(tile)));
        NotifyPropertyChanged(nameof(Word));
        NotifyPropertyChanged(nameof(Tiles));
    }
}
