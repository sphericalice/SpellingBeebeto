using CommunityToolkit.Mvvm.Input;
using SpellingBeebeto.Models;
using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SpellingBeebeto.ViewModels;

public class GameBoardVM : BindableBase
{
    public enum AnimationState
    {
        NotAnimating,
        CorrectAnswer,
        IncorrectAnswer,
    }

    private readonly GameBoard Model;

    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string Message => "Welcome to Spelling Beebeto";
    public WordVM Word { get; private set; }
    public ObservableCollection<TileVM> Tiles { get; private set; }
    public IRelayCommand DeleteLetterCommand { get; }
    public IRelayCommand ShuffleTilesCommand { get; }
    public IRelayCommand SubmitWordCommand { get; }
    public AnimationState CurrentAnimationState { get; internal set; } = AnimationState.NotAnimating;

    public GameBoardVM(GameBoard model)
    {
        Model = model;
        Word = new(Model.Word);
        Tiles = UpdateTiles();
        Model.PropertyChanged += UpdateVM;

        DeleteLetterCommand = new RelayCommand(Model.DeleteLastLetter);
        ShuffleTilesCommand = new RelayCommand(Model.ShuffleTiles);
        SubmitWordCommand = new RelayCommand(SubmitWord);
    }
    private ObservableCollection<TileVM> UpdateTiles() => new(Model.Tiles.Select(tile => new TileVM(tile) { GameBoard = this }));
    public void SubmitWord()
    {
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
