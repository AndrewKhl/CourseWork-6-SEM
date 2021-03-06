﻿using System;
using System.IO;
using System.Windows;

namespace Watcher
{
    public class LoggerManager
    {
        private StreamWriter _streamWriter;
        private string _loggerFile;
        private bool _showMessageWindows = true;


        public LoggerManager(string logFile)
        {
            _loggerFile = logFile;
        }

        public void Start(bool show)
        {
            _showMessageWindows = show;
            _streamWriter = new StreamWriter(_loggerFile, true);
        }

        public void LogBadProcess(string procName, ServerManager client = null)
        {
            var message = Properties.Resources.UnregProcess + $" {procName}";
            WriteMessageInFile("PROCESS", message);

            if (_showMessageWindows)
                MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            client?.SendMessage(message);
        }

        public void LogUnregistredUSB(string usbName, ServerManager client = null)
        {
            var message = Properties.Resources.UnregDevice + $" = {usbName}";

            WriteMessageInFile("DEVICE", message);

            if (_showMessageWindows)
                MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            client?.SendMessage(message);
        }

        public void LogError(string message)
        {
            WriteMessageInFile("ERROR", message);
        }

        public void LogMessage(string counterName, double size, double maxSize, string pref, ServerManager client = null)
        {
            var message = Properties.Resources.PerformCounter + $" {counterName} " + Properties.Resources.MaxAllowed + $" {size:F3}{pref} ({maxSize}{pref})";
            WriteMessageInFile("INFO", message);

            if (_showMessageWindows)
                MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            client?.SendMessage(message);
        }

        public void Close()
        {
            _streamWriter?.Dispose();
            _streamWriter = null;
        }

        private void WriteMessageInFile(string type, string message)
        {
            _streamWriter?.WriteLine($"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} {type.PadRight(10, ' ')} {Environment.MachineName.PadRight(10, ' ')} {message}");
        }
    }
}
