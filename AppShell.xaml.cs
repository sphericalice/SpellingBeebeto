﻿namespace SpellingBeebeto;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Views.Home), typeof(Views.Home));
        Routing.RegisterRoute(nameof(Views.GameBoard), typeof(Views.GameBoard));
    }
}
