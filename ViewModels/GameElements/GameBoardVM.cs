﻿using CommunityToolkit.Mvvm.Input;
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

    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public WordVM Word { get; private set; }
    public TileVM KeyTile { get; private set; }
    public ObservableCollection<TileVM> Tiles { get; private set; }
    public ObservableCollection<string> LatestWords { get; private set; }
    public ObservableCollection<string> AcceptedWords => Model.AcceptedWords;
    public string RejectionMessage => RejectionMessages[Model.Validity];
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
        AcceptedWords.CollectionChanged += (o, e) =>
        {
            LatestWords = new(Model.AcceptedWords.Take(Config.LatestWordsLength));
            NotifyPropertyChanged(nameof(LatestWords));
        };

        DeleteLetterCommand = new RelayCommand(Model.DeleteLastLetter);
        ShuffleTilesCommand = new RelayCommand(ShuffleTiles);
        SubmitWordCommand = new RelayCommand(SubmitWord);
    }
    private ObservableCollection<TileVM> UpdateTiles() => new(Model.Tiles.Select(tile => new TileVM(tile) { GameBoard = this }));
    public void ShuffleTiles()
    {
        Model.ShuffleTiles();
    }
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
