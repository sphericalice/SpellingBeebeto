namespace SpellingBeebeto.ViewModels;

internal class GameBoardVM
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string Message => "Welcome to Spelling Beebeto";

    public GameBoardVM()
    {
    }
}
