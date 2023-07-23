using CommunityToolkit.Maui;

namespace SpellingBeebeto;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterViewModels()
            .RegisterViews()
            .RegisterModels();

        return builder.Build();
    }
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ViewModels.GameElements.GameBoardVM>();

        return mauiAppBuilder;
    }
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<Views.GameBoard>();

        return mauiAppBuilder;
    }
    public static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<Models.GameElements.GameBoard>();

        return mauiAppBuilder;
    }
}
