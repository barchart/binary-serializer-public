/*
    usage example:
    dotnet script Publish.csx -- --api-key "YourApiKey"
*/

#r "nuget: Newtonsoft.Json, 13.0.1"
#r "nuget: System.CommandLine, 2.0.0-beta1"

#region Using Statements

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Invocation;

#endregion

#region Methods
void RunCommand(string command, string args)
{
    var process = new System.Diagnostics.Process
    {
        StartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = command,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.Start();
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();
    process.WaitForExit();

    Console.WriteLine(output);
    if (!string.IsNullOrEmpty(error))
    {
        Console.WriteLine("Error: " + error);
    }
}

#endregion

string projectDirectory = "../Barchart.BinarySerializer";

RunCommand("dotnet", $"build {projectDirectory} -c Release");

RunCommand("dotnet", $"pack {projectDirectory} -c Release --output {projectDirectory}/output");

var packagePath = Directory.GetFiles($"{projectDirectory}/output", "*.nupkg", SearchOption.AllDirectories).FirstOrDefault();

if (packagePath == null)
{
    Console.WriteLine("No .nupkg file found in the output directory.");
}
else
{
    var rootCommand = new RootCommand
    {
        new Option<string>(
            "--api-key",
            description: "The API key for NuGet")
    };

    rootCommand.Handler = CommandHandler.Create<string>((apiKey) =>
    {
        Console.WriteLine("Pushing package to NuGet...");
        string source = "https://api.nuget.org/v3/index.json";
        RunCommand("dotnet", $"nuget push {packagePath} --api-key {apiKey} --source {source}");
    });

    rootCommand.Invoke(Args.ToArray());
}