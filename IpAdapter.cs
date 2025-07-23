using Spectre.Console;
using Spectre.Console;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.NetworkInformation;

namespace Network.Manager.Console.App;

public static class IpAdapter
{
    public static void DisplayCurrentIP()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]=== Current IP Settings ===[/]");
        UiComponent.ShowLoadingAnimation();
        try
        {
            var table = new Table();
            table.AddColumn("Adapter");
            table.AddColumn("IP");
            table.AddColumn("Subnet Mask");
            table.AddColumn("Gateway");
            table.Border(TableBorder.Minimal);

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters.Where(a => a.OperationalStatus == OperationalStatus.Up))
            {
                var properties = adapter.GetIPProperties();
                foreach (var ip in properties.UnicastAddresses.Where(a =>
                             a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    var gateway = properties.GatewayAddresses.FirstOrDefault()?.Address.ToString() ?? "-";
                    table.AddRow($"[cyan]{adapter.Name}[/]", ip.Address.ToString(), ip.IPv4Mask?.ToString() ?? "-",
                        gateway);
                }
            }

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red][!] Error fetching IP: {ex.Message}[/]");
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        System.Console.ReadKey();
    }


    public static void SetNewIP()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]=== Set New IP ===[/]");
        NetworkAdapters.DisplayAvailableAdapters();
        try
        {
            var adapter = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter network adapter name:[/]")
                    .Validate(input =>
                        string.IsNullOrWhiteSpace(input)
                            ? ValidationResult.Error("[red]Adapter name cannot be empty![/]")
                            : ValidationResult.Success()));
            var useDhcp = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Use DHCP? (y/n):[/]")
                    .Validate(input =>
                        input.ToLower() is "y" or "n"
                            ? ValidationResult.Error("[red]Please enter 'y' or 'n'![/]")
                            : ValidationResult.Success())
            ).ToLower() == "y";

            if (useDhcp)
            {
                UiComponent.ShowLoadingAnimation("Applying DHCP settings");
                string command = $"interface ip set address name=\"{adapter}\" source=dhcp";
                NetshCommandService.ExecuteNetshCommand(command);
                AnsiConsole.MarkupLine("[green][✔] Switched to DHCP successfully![/]");
                DisplayCurrentIP();
            }
            else
            {
                var ipAddress = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Enter IP address (e.g., 192.168.1.100):[/]")
                        .Validate(input =>
                            string.IsNullOrWhiteSpace(input)
                                ? ValidationResult.Error("[red]IP address cannot be empty![/]")
                                : ValidationResult.Success()));
                var subnetMask = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Enter subnet mask (e.g., 255.255.255.0):[/]")
                        .Validate(input =>
                            string.IsNullOrWhiteSpace(input)
                                ? ValidationResult.Error("[red]Subnet mask cannot be empty![/]")
                                : ValidationResult.Success()));
                var gateway = AnsiConsole.Prompt(
                    new TextPrompt<string>("[green]Enter default gateway (optional, press Enter to skip):[/]")
                        .AllowEmpty());

                UiComponent.ShowLoadingAnimation("Applying static IP settings");

                string command =
                    $"interface ip set address name=\"{adapter}\" source=static addr={ipAddress} mask={subnetMask}";
                if (!string.IsNullOrEmpty(gateway))
                    command += $" gateway={gateway} gwmetric=0";
                NetshCommandService.ExecuteNetshCommand(command);
                AnsiConsole.MarkupLine("[green][✔] IP set successfully![/]");
                DisplayCurrentIP();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red][!] Error setting IP: {ex.Message}[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            System.Console.ReadKey();
        }
    }
}