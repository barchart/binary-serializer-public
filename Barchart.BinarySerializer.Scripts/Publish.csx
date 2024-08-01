/*
    usage example:
    dotnet script Publish.csx -- --api-key "YourApiKey" --git-token "YourGitToken" --version-increment VERSION

    VERSION: "MAJOR", "MINOR", "PATCH"
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
using System.Xml.Linq;

using Process = System.Diagnostics.Process;

#endregion

#region Methods
void RunCommand(string command, string args)
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
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

string IncrementVersion(string version, string incrementType)
{
    var versionParts = version.Split('.').Select(int.Parse).ToArray();
    
    switch (incrementType.ToLower())
    {
        case "major":
            versionParts[0]++;
            versionParts[1] = 0;
            versionParts[2] = 0;
            break;
        case "minor":
            versionParts[1]++;
            versionParts[2] = 0;
            break;
        case "patch":
            versionParts[2]++;
            break;
        default:
            throw new ArgumentException("Invalid increment type. Must be 'major', 'minor', or 'patch'.");
    }

    return string.Join('.', versionParts);
}

void UpdateCsprojVersion(string csprojPath, string newVersion)
{
    var doc = XDocument.Load(csprojPath);
    var versionElement = doc.Descendants("Version").FirstOrDefault();
    if (versionElement != null)
    {
        versionElement.Value = newVersion;
        doc.Save(csprojPath);
    }
}

string ReadReleaseDescription(string version)
{
    string descriptionFilePath = $"../.releases/{version}.md";
    
    return File.ReadAllText(descriptionFilePath);
}

void CreateGitHubRelease(string gitToken, string newVersion, string releaseDescription)
{
    string command = "gh";
    string args = $"release create {newVersion} --title \"v{newVersion}\" --notes \"{releaseDescription}\"";

    RunCommand(command, args);
}

#endregion

string projectDirectory = "../Barchart.BinarySerializer";
string csprojPath = $"{projectDirectory}/Barchart.BinarySerializer.csproj";

var rootCommand = new RootCommand
{
    new Option<string>(
        "--api-key",
        description: "The API key for NuGet"),
    new Option<string>(
        "--git-token",
        description: "The GitHub token"),
    new Option<string>(
        "--version-increment",
        description: "The version increment type: major, minor, patch",
        getDefaultValue: () => "patch")
};

rootCommand.Handler = CommandHandler.Create<string, string, string>((apiKey, gitToken, versionIncrement) =>
{
    var doc = XDocument.Load(csprojPath);
    var versionElement = doc.Descendants("Version").FirstOrDefault();
    var currentVersion = versionElement?.Value ?? "0.0.0";

    var newVersion = IncrementVersion(currentVersion, versionIncrement);
    Console.WriteLine($"Version incremented from {currentVersion} to {newVersion}");

    UpdateCsprojVersion(csprojPath, newVersion);

    RunCommand("dotnet", $"build {projectDirectory} -c Release");
    RunCommand("dotnet", $"pack {projectDirectory} -c Release --output {projectDirectory}/output");

    var packagePath = Directory.GetFiles($"{projectDirectory}/output", "*.nupkg", SearchOption.AllDirectories).FirstOrDefault();

    if (packagePath == null)
    {
        Console.WriteLine("No .nupkg file found in the output directory.");
        return;
    }

    Console.WriteLine("Publishing release to GitHub...");
    string releaseDescription = ReadReleaseDescription(newVersion);
    CreateGitHubRelease(gitToken, newVersion, releaseDescription);
    
    Console.WriteLine("Pushing package to NuGet...");
    string source = "https://api.nuget.org/v3/index.json";
    RunCommand("dotnet", $"nuget push {packagePath} --api-key {apiKey} --source {source}");
});

rootCommand.Invoke(Args.ToArray());