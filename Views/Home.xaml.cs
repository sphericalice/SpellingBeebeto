using SpellingBeebeto.ViewModels.GameElements;

namespace SpellingBeebeto.Views;

public partial class Home : ContentPage
{
	public Home(HomeVM viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
	}
}
