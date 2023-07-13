using SpellingBeebeto.Views;

namespace SpellingBeebeto;

public partial class App : Application
{
    private readonly GameBoard GameBoard;
    public App(GameBoard gameBoard)
	{
		InitializeComponent();

        this.GameBoard = gameBoard;
    }
    protected override Window CreateWindow(IActivationState activationState)
    {
        return new()
        {
            MaximumHeight = 750,
            MaximumWidth = 400,
            Page = GameBoard
        };
    }
}
