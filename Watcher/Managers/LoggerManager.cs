﻿using System;
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
        private string _loggerFile;

        public LoggerManager(string logFile)
        {
            _loggerFile = logFile;
        }

        public void Start()
        {
            _streamWriter = new StreamWriter(_loggerFile, true);
        }

        public void LogBadProcess(string procName)
        {
            var message = $"Unregistered process detected {procName}";
            WriteMessageInFile("PROCESS", message);

            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            _streamWriter = null;
        }

        private void WriteMessageInFile(string type, string message)
        {
            _streamWriter?.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} {type.PadRight(10, ' ')} {Environment.MachineName.PadRight(10, ' ')} {message}");
        }
    }
}
