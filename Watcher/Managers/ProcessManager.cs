using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    public class ProcessManager
    {
        private const int DurationLastCheck = 5;

        private string _processesFile;
        private int _currentCheck = 0;
        private LoggerManager _logger;

        public ObservableCollection<string> GoodProcess { get; }

        public ProcessManager(string procFile, LoggerManager logger)
        {
            GoodProcess = new ObservableCollection<string>();

            _processesFile = procFile;
            _logger = logger;

            LoadGoodProcessesWithFile();
        }

        public void AddAllSystemProcess()
        {
            GoodProcess.Clear();

            foreach (Process proc in Process.GetProcesses())
                AddProcess(proc.ProcessName);
        }

        public void DeletedSelectProcess(string name)
        {
            if (GoodProcess.Contains(name))
                GoodProcess.Remove(name);
        }

        public void DeleteAllProcesses()
        {
            GoodProcess.Clear();
        }

        public void AddProcess(string name)
        {
            if (!GoodProcess.Contains(name))
                GoodProcess.Add(name);
        }

        private void LoadGoodProcessesWithFile()
        {
            if (File.Exists(_processesFile))
            {
                foreach (string proc in File.ReadAllLines(_processesFile, Encoding.Default))
                    AddProcess(proc);
            }
        }

        public void SaveGoodProcessesInFile()
        {
            using (var fs = new FileStream(_processesFile, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    foreach (var name in GoodProcess)
                        sw.WriteLine(name);
                }
            }
        }

        public void CheckSystemProcess()
        {
            if (++_currentCheck == DurationLastCheck)
            {
                _currentCheck = 0;
                foreach (var proc in Process.GetProcesses())
                    if (!GoodProcess.Contains(proc.ProcessName) && proc.ProcessName != "backgroundTaskHost")
                        _logger.LogBadProcess(proc.ProcessName);
            }
        }
    }
}
