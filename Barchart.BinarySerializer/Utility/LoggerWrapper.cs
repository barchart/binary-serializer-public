using Serilog;
namespace Barchart.BinarySerializer.Utility
{
    /// <summary>
    /// Provides utility methods for logging using Serilog.
    /// </summary>
    public static class LoggerWrapper
    {
        public static void InitializeLogger()
        {
            string logFilePath = "../../../../Logs/log-.txt";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();
        }

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
    }
}