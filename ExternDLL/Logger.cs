using System;
using System.IO;

namespace ExternDLL
{
    public class Logger
    {
        private string logDirectory;

        public Logger(string logDirectory)
        {
            this.logDirectory = logDirectory;

            // Stellen Sie sicher, dass das Protokollverzeichnis vorhanden ist
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public void LogRequest(string method, string parameters)
        {
            string logFileName = GetLogFileName();

            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - Method: {method}, Parameters: {parameters}";

            File.AppendAllText(logFileName, logMessage + Environment.NewLine);
        }


        private string GetLogFileName()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string logFileName = Path.Combine(logDirectory, $"log_{currentDate}.txt");
            return logFileName;
        }
    }
}