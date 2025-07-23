using System.Diagnostics;
using Spectre.Console;

namespace Network.Manager.Console.App;

public static class NetshCommandService
{
    public static void ExecuteNetshCommand(string command)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo
        {
            FileName = "netsh",
            Arguments = command,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8
        };

        using (Process process = Process.Start(processInfo))
        {
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"netsh error: {error.Trim()}\nOutput: {output.Trim()}");
            }

            if (!string.IsNullOrEmpty(output))
            {
                AnsiConsole.MarkupLine($"[grey]netsh output: {output.Trim()}[/]");
            }
        }
    }
}