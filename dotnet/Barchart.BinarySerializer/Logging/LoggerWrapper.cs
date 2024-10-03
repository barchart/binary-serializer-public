#region Using Statements

using Serilog;

#endregion

namespace Barchart.BinarySerializer.Logging;

/// <summary>
///     Provides utility methods for logging using Serilog.
/// </summary>
public class LoggerWrapper
{
    #region Fields

    private static readonly LoggerWrapper? Instance = null;

    #endregion

    #region Constructor(s)

    private LoggerWrapper() {}

    #endregion

    #region Methods

    /// <summary>
    ///     Initializes the logger with a custom log file path.
    /// </summary>
    /// <param name="logFilePath">
    ///     The path to the log file.
    /// </param>
    public static void Initialize(string logFilePath)
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("Logger is already initialized.");
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

    }

    /// <summary>
    ///     Logs information level messages. This method should be used for general informational messages.
    /// </summary>
    /// <param name="text">
    ///     The message to log.
    /// </param>
    public static void LogInformation(string text)
    {
        EnsureInitialized();
        Log.Information(text);
    }

    /// <summary>
    ///     Logs debug level messages. This method should be used for messages that are useful in debugging scenarios.
    /// </summary>
    /// <param name="text">
    ///     The message to log.
    /// </param>
    public static void LogDebug(string text)
    {
        EnsureInitialized();
        Log.Debug(text);
    }

    /// <summary>
    ///     Logs warning level messages. This method should be used for messages that signify a potential issue or important notice.
    /// </summary>
    /// <param name="text">
    ///     The message to log.
    /// </param>
    public static void LogWarning(string text)
    {
        EnsureInitialized();
        Log.Warning(text);
    }

    /// <summary>
    ///     Logs error level messages. This method should be used for messages that signify an error has occurred.
    /// </summary>
    /// <param name="text">
    ///     The message to log.
    /// </param>
    public static void LogError(string text)
    {
        EnsureInitialized();
        Log.Error(text);
    }

    private static void EnsureInitialized()
    {
        if (Instance == null)
        {
            throw new InvalidOperationException("Logger is not initialized. Call Initialize() with a log file path first.");
        }
    }

    #endregion
}