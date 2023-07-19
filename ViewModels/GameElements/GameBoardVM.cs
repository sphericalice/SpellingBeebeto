using CommunityToolkit.Mvvm.Input;
using SpellingBeebeto.Models.GameElements;
using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SpellingBeebeto.ViewModels.GameElements;

public class GameBoardVM : BindableBase
{
    private readonly GameBoard Model;

    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public WordVM Word { get; private set; }
    public TileVM KeyTile { get; private set; }
    public ObservableCollection<TileVM> Tiles { get; private set; }
    public ObservableCollection<string> AcceptedWords => Model.AcceptedWords;
    public IRelayCommand DeleteLetterCommand { get; }
    public IRelayCommand ShuffleTilesCommand { get; }
    public IRelayCommand SubmitWordCommand { get; }
    public AnimationState CurrentAnimationState { get; internal set; } = AnimationState.NotAnimating;

    public GameBoardVM(GameBoard model)
    {
        Model = model;
        Word = new(Model.Word);
        KeyTile = new(Model.KeyTile) { GameBoard = this };
        Tiles = UpdateTiles();
        Model.PropertyChanged += UpdateVM;

        DeleteLetterCommand = new RelayCommand(Model.DeleteLastLetter);
        ShuffleTilesCommand = new RelayCommand(Model.ShuffleTiles);
        SubmitWordCommand = new RelayCommand(SubmitWord);
    }
    private ObservableCollection<TileVM> UpdateTiles() => new(Model.Tiles.Select(tile => new TileVM(tile) { GameBoard = this }));

    public void SubmitWord()
    {
        if (Model.WordIsEmpty()) return;
        CurrentAnimationState = Model.CanSubmitWord() ? AnimationState.CorrectAnswer : AnimationState.IncorrectAnswer;
        NotifyPropertyChanged(nameof(Word));
    }

    private void UpdateVM(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Word)) NotifyPropertyChanged(nameof(Word));
        if (e.PropertyName == nameof(Tiles))
        {
            Tiles = UpdateTiles();
            NotifyPropertyChanged(nameof(Tiles));
        }
        if (e.PropertyName == nameof(AcceptedWords)) NotifyPropertyChanged(nameof(AcceptedWords));
    }

    internal void NotifyAnimationComplete()
    {
        CurrentAnimationState = AnimationState.NotAnimating;
        Model.SubmitWord();
    }

    internal void RejectOverlyLongWord()
    {
        if (Model.WordIsTooLong())
            SubmitWord();
    }

    internal bool CanAddTile() => CurrentAnimationState == AnimationState.NotAnimating;
}
