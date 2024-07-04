/*
    usage example:
    dotnet script test.csx
*/

using System;
using System.Diagnostics;

string projectPath = "../Barchart.BinarySerializer.Tests"; 

ProcessStartInfo psi = new ProcessStartInfo
{
    FileName = "dotnet",
    Arguments = $"test",
    WorkingDirectory = projectPath,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    CreateNoWindow = true
};

Console.WriteLine($"Running tests in {projectPath}");

using (Process process = Process.Start(psi))
{
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();

    Console.WriteLine(output);
    if (!string.IsNullOrEmpty(error))
    {
        Console.WriteLine($"Error: {error}");
    }

    process.WaitForExit();
}