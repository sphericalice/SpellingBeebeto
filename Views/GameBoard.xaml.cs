using SpellingBeebeto.ViewModels;

namespace SpellingBeebeto.Views;

public partial class GameBoard : ContentPage
{
    public GameBoard(GameBoardVM viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}