using SpellingBeebeto.Utilities;
using SpellingBeebeto.ViewModels.GameElements;
using System.ComponentModel;

namespace SpellingBeebeto.Views;

public partial class GameBoard : ContentPage
{
    public GameBoard(GameBoardVM viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        viewModel.PropertyChanged += GameBoardVM_PropertyChanged;
    }

    private static Animation Shake(VisualElement view) => new()
    {
        { 0, 0.125, new Animation (v => view.TranslationX = v, 0, -13) },
        { 0.125, 0.250, new Animation(v => view.TranslationX = v, -13, 13) },
        { 0.250, 0.375, new Animation(v => view.TranslationX = v, 13, -11) },
        { 0.375, 0.5, new Animation(v => view.TranslationX = v, -11, 11) },
        { 0.5, 0.625, new Animation(v => view.TranslationX = v, 11, -7) },
        { 0.625, 0.75, new Animation(v => view.TranslationX = v, -7, 7) },
        { 0.75, 0.875, new Animation(v => view.TranslationX = v, 7, -5) },
        { 0.875, 1, new Animation(v => view.TranslationX = v, -5, 0) },
    };

    private static Animation Fade(VisualElement view) => new(v => view.FadeTo(20, 1));

    private readonly Dictionary<AnimationState, Action<GameBoardVM, VisualElement>> animations = new()
    {
        [AnimationState.CorrectAnswer] = (viewModel, visualElement) => visualElement.Animate("Fade", Fade(visualElement), length: 20, easing: Easing.CubicOut, finished: (v, c) => viewModel.NotifyAnimationComplete()),
        [AnimationState.IncorrectAnswer] = (viewModel, visualElement) => visualElement.Animate("Shake", Shake(visualElement), length: 500, easing: Easing.Linear, finished: (v, c) => viewModel.NotifyAnimationComplete()),
    };

    private void GameBoardVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is not GameBoardVM viewModel) return;
        if (e.PropertyName == nameof(viewModel.WordListCollapsed)) DynamicRow.Height = viewModel.WordListCollapsed ? new GridLength(1, GridUnitType.Auto) : new GridLength(1, GridUnitType.Star);
        AnimationState animationState = viewModel.CurrentAnimationState;
        if (animationState == AnimationState.Animating || animationState == AnimationState.NotAnimating) return;
        animations[animationState](viewModel, WordLabel);
        viewModel.CurrentAnimationState = AnimationState.Animating;
    }
}
