using Serilog;

namespace CommonSharedLibrary.CommonLogger;

public static class LogException
{
    public static void LogExceptions(Exception ex)
    {
        LogToFile(ex.Message);
        LogToConsole(ex.Message);
        LogToDebugger(ex.Message);
    }

    private static void LogToFile(string message) => Log.Information(message);
    private static void LogToConsole(string message) => Log.Information(message);
    private static void LogToDebugger(string message) => Log.Information(message);
}