using Spectre.Console;

namespace Network.Manager.Console.App;

public static class UiComponent
{
    public static void DisplayWelcomeAnimation()
    {
        AnsiConsole.MarkupLine("[cyan]====================================[/]");
        AnsiConsole.Markup("Starting [bold yellow]Network Manager[/]");
        for (int i = 0; i < 3; i++)
        {
            AnsiConsole.Markup(".");
            Thread.Sleep(300);
        }

        AnsiConsole.MarkupLine("\n[cyan]====================================[/]");
        Thread.Sleep(500);
    }


    public static void ShowExitAnimation()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[cyan]====================================[/]");
        AnsiConsole.Markup("Thank you for using [bold yellow]Network Manager[/]");
        for (int i = 0; i < 3; i++)
        {
            AnsiConsole.Markup(".");
            Thread.Sleep(300);
        }

        AnsiConsole.MarkupLine("\n[cyan]====================================[/]");
        Thread.Sleep(1000);
    }

    public static void DisplayMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[green]=== Network Manager ===[/]");
        AnsiConsole.WriteLine("1. Show Current DNS");
        AnsiConsole.WriteLine("2. Set New DNS");
        AnsiConsole.WriteLine("3. Show Current IP");
        AnsiConsole.WriteLine("4. Set New IP (Static/DHCP)");
        AnsiConsole.WriteLine("5. Enable/Disable Network Adapter");
        AnsiConsole.WriteLine("6. Exit");
        AnsiConsole.MarkupLine("[grey]------------------------- [/]");
    }


    public static void ShowLoadingAnimation(string message = "Loading")
    {
        AnsiConsole.Markup($"[magenta]{message}[/]");
        for (int i = 0; i < 3; i++)
        {
            AnsiConsole.Markup(".");
            Thread.Sleep(300);
        }

        AnsiConsole.WriteLine();
    }
}