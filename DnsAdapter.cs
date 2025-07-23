using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Network.Manager.Console.App;

public static class DnsAdapter
{
    public static void DisplayCurrentDNS()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]=== Current DNS Settings ===[/]");
        UiComponent.ShowLoadingAnimation();
        try
        {
            var table = new Table();
            table.AddColumn("Adapter");
            table.AddColumn("DNS");
            table.Border(TableBorder.Minimal);

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters.Where(a => a.OperationalStatus == OperationalStatus.Up))
            {
                var dnsAddresses = adapter.GetIPProperties().DnsAddresses;
                if (dnsAddresses.Count == 0)
                {
                    table.AddRow($"[cyan]{adapter.Name}[/]", "-");
                }
                else
                {
                    foreach (var dns in dnsAddresses)
                    {
                        table.AddRow($"[cyan]{adapter.Name}[/]", dns.ToString());
                    }
                }
            }

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red][!] Error fetching DNS: {ex.Message}[/]");
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        System.Console.ReadKey();
    }

    public static void SetNewDNS()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]=== Set New DNS ===[/]");
        NetworkAdapters.DisplayAvailableAdapters();
        try
        {
            var adapter = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter network adapter name:[/]")
                    .Validate(input =>
                        string.IsNullOrWhiteSpace(input)
                            ? ValidationResult.Error("[red]Adapter name cannot be empty![/]")
                            : ValidationResult.Success()));
            var primary = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter primary DNS (e.g., 8.8.8.8):[/]")
                    .Validate(input =>
                        string.IsNullOrWhiteSpace(input)
                            ? ValidationResult.Error("[red]Primary DNS cannot be empty![/]")
                            : ValidationResult.Success()));
            var secondary = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter secondary DNS (optional, press Enter to skip):[/]")
                    .AllowEmpty());

            UiComponent.ShowLoadingAnimation("Applying DNS settings");

            string command = $"interface ip set dns name=\"{adapter}\" source=static addr={primary}";
            NetshCommandService.ExecuteNetshCommand(command);

            if (!string.IsNullOrEmpty(secondary))
            {
                command = $"interface ip add dns name=\"{adapter}\" addr={secondary} index=2";
                NetshCommandService.ExecuteNetshCommand(command);
            }

            AnsiConsole.MarkupLine("[green][✔] DNS set successfully![/]");
            DisplayCurrentDNS();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red][!] Error setting DNS: {ex.Message}[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            System.Console.ReadKey();
        }
    }
}