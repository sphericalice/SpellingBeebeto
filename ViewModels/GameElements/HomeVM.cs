using CommunityToolkit.Mvvm.Input;

namespace SpellingBeebeto.ViewModels.GameElements
{
    public class HomeVM
    {
        public string Title => AppInfo.Name;
        public string Version => AppInfo.VersionString;
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
