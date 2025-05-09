using System.Diagnostics;
using Serilog;

namespace CommonSharedLibrary.CommonLogger.Serilog;

public static class CommonLogger
{
    /// <summary>
    /// Write information initial log
    /// </summary>
    /// <param name="log"></param>
    /// <param name="name"></param>
    /// <param name="stopwatch"></param>
    public static void LogInitialized(ILogger log, string name, Stopwatch stopwatch)
    {
        stopwatch.Start();
        log.Information($"Initialized: {name}.");
    }
    
}