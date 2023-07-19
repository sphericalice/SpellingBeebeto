using SpellingBeebeto.Views;

namespace SpellingBeebeto;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
