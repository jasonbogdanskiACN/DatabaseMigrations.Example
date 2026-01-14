using DbUp;
using DbUp.Engine;
using DbUp.Support;
using DbUp.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

// Minimal console example demonstrating DbUp usage with embedded SQL scripts.
class Program
{
    static int Main(string[] args)
    {
        var connectionString = args.Length > 0
            ? args[0]
            : @"Server=(localdb)\MSSQLLocalDB;Database=DbUpExample;Trusted_Connection=True;";

        Console.WriteLine($"Using connection string: {connectionString}");

        // Create database if it doesn't exist
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.StartsWith("DbUp.Example.Migrations."), new SqlScriptOptions { ScriptType = ScriptType.RunOnce, RunGroupOrder = 1 })
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.StartsWith("DbUp.Example.Permissions."), new SqlScriptOptions { ScriptType = ScriptType.RunAlways, RunGroupOrder = 2 })
            .LogToConsole()
            .WithTransactionPerScript()
            .Build();

        // Generate an upgrade HTML report before executing. Optional --reportPath <path> overrides destination.
        try
        {
            var reportPath = GetArgValue(args, "--reportPath") ?? Path.Combine(AppContext.BaseDirectory, $"UpgradeReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            upgrader.GenerateUpgradeHtmlReport(reportPath);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Upgrade report generated: {reportPath}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning: failed to generate upgrade report: {ex.Message}");
            Console.ResetColor();
        }

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            return -1;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Database upgrade successful.");
        Console.ResetColor();

        return 0;
    }

    static string? GetArgValue(string[] args, string name)
    {
        if (args == null || args.Length == 0) return null;
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], name, StringComparison.InvariantCultureIgnoreCase) && i + 1 < args.Length)
                return args[i + 1];
        }
        return null;
    }
}
