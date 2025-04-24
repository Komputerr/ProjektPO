using System;
using System.IO;

namespace krol_bartosz_kwaskiewicz_nataniel.classes
{
    public static class Logger
    {
        private static readonly string logFilePath = "app_logs.txt";

        static Logger()
        {
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
        }

        public static void Log(string message)
        {
            string logEntry = $"[{DateTime.Now}] {message}";
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }

        public static void LogAction(string username, string action)
        {
            Log($"User '{username}' performed action: {action}");
        }

        public static void LogError(string errorMessage)
        {
            Log($"ERROR: {errorMessage}");
        }

        public static void LogLogin(string username, bool success)
        {
            Log($"Login attempt for user '{username}' - {(success ? "SUCCESS" : "FAILED")}");
        }

        public static void LogRegistration(string username)
        {
            Log($"New user registered: '{username}'");
        }

        public static void LogPostOperation(string username, string platform, string operation)
        {
            Log($"User '{username}' {operation} on {platform}");
        }
        public static void DisplayLogsInConsole()
        {
            try
            {
                if (!File.Exists(logFilePath))
                {
                    Console.WriteLine("No log file exists yet.");
                    return;
                }

                string title = $"APPLICATION LOGS";
                int padding = (78 - title.Length) / 2;

                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"║{new string(' ', padding)}{title}{new string(' ', 78 - title.Length - padding)}║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                var logLines = File.ReadAllLines(logFilePath);

                foreach (var line in logLines)
                {
                    var formattedLine = line.Length > 76 ? line.Substring(0, 73) + "..." : line;
                    Console.WriteLine($"║ {formattedLine.PadRight(76)} ║");
                }

                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine($"\nShowing {logLines.Length} total log entries.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying logs: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}