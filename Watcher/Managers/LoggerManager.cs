using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Watcher
{
    public class LoggerManager
    {
        private StreamWriter _streamWriter;

        public LoggerManager(string logFile)
        {
            _streamWriter = new StreamWriter(logFile, true);
        }

        public void LogError(string message)
        {
            WriteMessageInFile("ERROR", message);
        }

        public void LogMessage(string counterName, double size, double maxSize, string pref)
        {
            var message = $"Performance counter {counterName} has exceeded the maximum allowed value {size:F3}{pref} ({maxSize}{pref})";
            WriteMessageInFile("INFO", message);

            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void Close()
        {
            _streamWriter?.Dispose();
        }

        private void WriteMessageInFile(string type, string message)
        {
            _streamWriter.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortDateString()} {type.PadRight(10, ' ')} {Environment.MachineName.PadLeft(10, ' ')} {message}");
        }
    }
}
