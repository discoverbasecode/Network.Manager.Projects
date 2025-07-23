using System.Diagnostics;
using System.Net.NetworkInformation;
using Spectre.Console;

namespace Network.Manager.Console.App;

abstract class Program
{
    static void Main(string[] args)
    {
        System.Console.Title = "Network Manager";
        UiComponent.DisplayWelcomeAnimation();

        while (true)
        {
            UiComponent.DisplayMenu();
            var choice = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Select an option (1-6):[/]")
                    .Validate(input => input switch
                    {
                        var x when string.IsNullOrWhiteSpace(x) => ValidationResult.Error(
                            "[red]Please enter a number![/]"),
                        var x when !int.TryParse(x, out int n) || n < 1 || n > 6 => ValidationResult.Error(
                            "[red]Please enter a number between 1 and 6![/]"),
                        _ => ValidationResult.Success()
                    }));

            switch (choice)
            {
                case "1":
                    DnsAdapter.DisplayCurrentDNS();
                    break;
                case "2":
                    DnsAdapter.SetNewDNS();
                    break;
                case "3":
                    IpAdapter.DisplayCurrentIP();
                    break;
                case "4":
                    IpAdapter.SetNewIP();
                    break;
                case "5":
                    NetworkAdapters.ToggleNetworkAdapter();
                    break;
                case "6":
                    UiComponent.ShowExitAnimation();
                    Environment.Exit(0);
                    break;
            }
        }
    }
}