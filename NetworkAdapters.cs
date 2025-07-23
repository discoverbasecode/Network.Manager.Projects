using Spectre.Console;
using System.Diagnostics;

namespace Network.Manager.Console.App;

public static class NetworkAdapters
{
    public static void DisplayAvailableAdapters()
    {
        AnsiConsole.MarkupLine("[yellow]=== Available Network Adapters ===[/]");
        UiComponent.ShowLoadingAnimation();
        try
        {
            var table = new Table();
            table.AddColumn("Adapter Name");
            table.AddColumn("Status");
            table.Border(TableBorder.Minimal);

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = "interface show interface",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8
            };

            using (Process process = Process.Start(processInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                string[] lines = output.Split('\n');
                foreach (string line in lines)
                {
                    if (line.Contains("Enabled") || line.Contains("Disabled"))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 4)
                        {
                            string status = parts[0];
                            string name = string.Join(" ", parts.Skip(3));
                            table.AddRow($"[cyan]{name}[/]", status);
                        }
                    }
                }
            }

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red][!] Error fetching adapters: {ex.Message}[/]");
        }

        AnsiConsole.WriteLine();
    }

    public static void ToggleNetworkAdapter()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]=== Enable/Disable Network Adapter ===[/]");
        NetworkAdapters.DisplayAvailableAdapters();
        try
        {
            var adapter = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter network adapter name:[/]")
                    .Validate(input =>
                        string.IsNullOrWhiteSpace(input)
                            ? ValidationResult.Error("[red]Adapter name cannot be empty![/]")
                            : ValidationResult.Success()));
            var action = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enable or Disable? (e/d):[/]")
                    .Validate(input =>
                        input.ToLower() is "e" or "d"
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Please enter 'e' or 'd'![/]"))
            ).ToLower();

            string command = action == "e"
                ? $"interface set interface \"{adapter}\" enable"
                : $"interface set interface \"{adapter}\" disable";
            UiComponent.ShowLoadingAnimation(action == "e" ? "Enabling adapter" : "Disabling adapter");
            NetshCommandService.ExecuteNetshCommand(command);

            AnsiConsole.MarkupLine(
                $"[green][✔] Adapter {adapter} {(action == "e" ? "enabled" : "disabled")} successfully![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red][!] Error: {ex.Message}[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            System.Console.ReadKey();
        }
    }
}