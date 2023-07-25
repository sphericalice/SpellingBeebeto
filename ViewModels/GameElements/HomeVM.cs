using CommunityToolkit.Mvvm.Input;

namespace SpellingBeebeto.ViewModels.GameElements
{
    public class HomeVM
    {
        public IRelayCommand PlayCommand { get; }
        public HomeVM()
        {
            PlayCommand = new AsyncRelayCommand(EnterGameBoardAsync);
        }

        private async Task EnterGameBoardAsync()
        {
            await Shell.Current.GoToAsync("GameBoard");
        }
    }
}
