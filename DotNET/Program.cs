// Minimal .NET 8 dispatcher for DotNET samples
// - Loads .env for PDFREST_API_KEY and optional PDFREST_URL
// - Dispatches to endpoint samples located under folder structure
//   * markdown-json: two-step upload + JSON call to markdown endpoint

using DotNetEnv;

static void PrintUsage()
{
    Console.Error.WriteLine("Usage: dotnet run -- <command> [args]\n");
    Console.Error.WriteLine("Commands:");
    Console.Error.WriteLine("  markdown-json <inputFile>        Upload then convert to Markdown (JSON two-step)");
    Console.Error.WriteLine("");
    Console.Error.WriteLine("Environment (.env supported):");
    Console.Error.WriteLine("  PDFREST_API_KEY=...              Required API key");
    Console.Error.WriteLine("  PDFREST_URL=...                  Optional base URL (e.g., https://eu-api.pdfrest.com for EU/GDPR)");
}

// Entry point
Env.Load();
var argv = Environment.GetCommandLineArgs().Skip(1).ToArray();
if (argv.Length == 0)
{
    PrintUsage();
    return;
}

var cmd = argv[0].Trim().ToLowerInvariant();
var rest = argv.Skip(1).ToArray();

switch (cmd)
{
    case "markdown-json":
        await Samples.EndpointExamples.JsonPayload.Markdown.Execute(rest);
        break;

    default:
        Console.Error.WriteLine($"Unknown command: {cmd}\n");
        PrintUsage();
        Environment.Exit(1);
        break;
}
