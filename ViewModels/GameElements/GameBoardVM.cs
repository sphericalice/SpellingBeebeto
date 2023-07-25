using CommunityToolkit.Mvvm.Input;
using SpellingBeebeto.Models.GameElements;
using SpellingBeebeto.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static SpellingBeebeto.Models.GameElements.WordValidity;
using Config = SpellingBeebeto.Models.GameConfiguration.GameConfiguration;

namespace SpellingBeebeto.ViewModels.GameElements;

public class GameBoardVM : BindableBase
{
    private readonly GameBoard Model;

    public string Word => Model.Word.Text.Replace(KeyTile.Letter.ToString(), $"<u>{KeyTile.Letter}</u>");
    public TileVM KeyTile { get; private set; }
    public ObservableCollection<TileVM> Tiles { get; private set; }
    public ObservableCollection<string> LatestWords { get; private set; }
    public ObservableCollection<string> AcceptedWords => Model.AcceptedWords;
    public ObservableCollection<string> SortedWords => new(AcceptedWords.OrderBy(s => s));
    public string RejectionMessage => RejectionMessages[Model.Validity];
    public bool WordListCollapsed { get; private set; } = true;
    public string WordsFoundText => $"You have found {AcceptedWords.Count} word{(AcceptedWords.Count == 1 ? "" : "s")}.";
    public IRelayCommand DeleteLetterCommand { get; }
    public IRelayCommand ShuffleTilesCommand { get; }
    public IRelayCommand SubmitWordCommand { get; }
    public IRelayCommand ToggleWordExpansionCommand { get; }
    public AnimationState CurrentAnimationState { get; internal set; } = AnimationState.NotAnimating;

    public GameBoardVM(GameBoard model)
    {
        Model = model;
        KeyTile = new(Model.KeyTile) { GameBoard = this };
        Tiles = UpdateTiles();
        Model.PropertyChanged += UpdateVM;
        LatestWords = new(Model.AcceptedWords.Take(Config.LatestWordsLength));
        AcceptedWords.CollectionChanged += (o, e) =>
        {
            LatestWords = new(Model.AcceptedWords.Take(Config.LatestWordsLength));
            NotifyPropertyChanged(nameof(LatestWords));
            NotifyPropertyChanged(nameof(WordsFoundText));
        };

        DeleteLetterCommand = new RelayCommand(Model.DeleteLastLetter);
        ShuffleTilesCommand = new RelayCommand(Model.ShuffleTiles);
        SubmitWordCommand = new RelayCommand(SubmitWord);
        ToggleWordExpansionCommand = new RelayCommand(ToggleWordExpansion);
    }

    private void ToggleWordExpansion()
    {
        WordListCollapsed = !WordListCollapsed;
        NotifyPropertyChanged(nameof(WordListCollapsed));
        NotifyPropertyChanged(nameof(SortedWords));
    }

    private ObservableCollection<TileVM> UpdateTiles() => new(Model.Tiles.Select(tile => new TileVM(tile) { GameBoard = this }));
    public void SubmitWord()
    {
        NotifyPropertyChanged(nameof(RejectionMessage));
        CurrentAnimationState = Model.GetWordValidity() switch
        {
            Validity.Empty => AnimationState.NotAnimating,
            Validity.Valid => AnimationState.CorrectAnswer,
            _ => AnimationState.IncorrectAnswer,
        };
        NotifyPropertyChanged(nameof(Word));
    }

    private void UpdateVM(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Word))
        {
            if (Model.IsWordTooLong()) RejectLongWord();
            NotifyPropertyChanged(nameof(Word));
        }
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
        NotifyPropertyChanged(nameof(RejectionMessage));
    }

    internal void RejectLongWord()
    {
        CurrentAnimationState = AnimationState.IncorrectAnswer;
        NotifyPropertyChanged(nameof(RejectionMessage));
    }

    internal bool CanClickTiles() => CurrentAnimationState == AnimationState.NotAnimating;
}
