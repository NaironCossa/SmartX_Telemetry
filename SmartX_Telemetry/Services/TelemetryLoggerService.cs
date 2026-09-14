using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SmartX_Telemetry.Services
{
    public static class TelemetryLoggerService
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "telemetry_system.log");

        public static void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public static void LogError(string message, Exception ex = null)
        {
            string detail = ex != null ? $"{message} | Exception: {ex.Message}" : message;
            WriteLog("ERROR", detail);
        }

        private static void WriteLog(string level, string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch
            {
                // Fallback to preserve runtime execution if disk access fails
            }
        }
    }
}