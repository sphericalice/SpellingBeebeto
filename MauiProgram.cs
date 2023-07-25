using CommunityToolkit.Maui;
using SpellingBeebeto.Models.GameElements;
using Config = SpellingBeebeto.Models.GameConfiguration.GameConfiguration;

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
            .RegisterSaveFile()
            .RegisterViewModels()
            .RegisterViews()
            .RegisterModels();

        return builder.Build();
    }
    public static MauiAppBuilder RegisterSaveFile(this MauiAppBuilder mauiAppBuilder)
    {
        RuleSet ruleSet = RuleSet.TryLoad();

        if (ruleSet is not null) mauiAppBuilder.Services.AddSingleton(ruleSet);
        else mauiAppBuilder.Services.AddSingleton<RuleSet>();

        return mauiAppBuilder;
    }
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ViewModels.GameElements.HomeVM>();
        mauiAppBuilder.Services.AddSingleton<ViewModels.GameElements.GameBoardVM>();

        return mauiAppBuilder;
    }
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<Views.Home>();
        mauiAppBuilder.Services.AddSingleton<Views.GameBoard>();

        return mauiAppBuilder;
    }
    public static MauiAppBuilder RegisterModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<Models.GameElements.GameBoard>();

        return mauiAppBuilder;
    }
}
