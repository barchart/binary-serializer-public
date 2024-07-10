#region Using Statements

using Serilog;

#endregion

namespace Barchart.BinarySerializer.Logging;

/// <summary>
///     Provides utility methods for logging using Serilog.
/// </summary>
public static class LoggerWrapper
{
    #region Constructor(s)

    static LoggerWrapper() {
        string logFilePath = "../../../../Logs/log-.txt";
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
        .CreateLogger();
    }

    #endregion

    #region Methods

    public static void LogInformation(string text)
    {
        Log.Information(text);
    }

    public static void LogDebug(string text)
    {
        Log.Debug(text);
    }

    public static void LogWarning(string text)
    {
        Log.Warning(text);
    }

    public static void LogError(string text)
    {
        Log.Error(text);
    }

    #endregion
}